using Brokers.Coinbase.Models;
using Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core;
using Org.Front.Core.Contracts.Helpers;
using Org.Front.Core.Contracts.Models.Brokers;

namespace Brokers.Coinbase.Services;

public class CoinbaseAuthService : BaseCoinbaseService, ICoinbaseAuthService
{
    private const string OAuthEndpoint = "https://api.coinbase.com/oauth/token";
    private const string UserPath = "v2/user";
    private const string AccountNameTemplate = "{0} wallets";
    private const string ClientIdNotSupportedErrorMessage = "Provided client id version is not supported.";
    private const string ClientSecretNotProvidedErrorMessage = "Coinbase client secret not provided.";
    private const int MaxNumberOfSymbolsToIncludeToAccountName = 4;

    public CoinbaseAuthService(HttpClient httpClient,
            ILogger<CoinbaseAuthService> logger,    
            IOptions<CoinbaseConfig> config)
        //IEventService eventService,
        //IDistributedCache cache)
        : base(httpClient,
            config,
            logger)
            //eventService,
            //cache)
    {
    }

    private const string DefaultScope =
        "wallet:accounts:read,wallet:transactions:read,wallet:payment-methods:read,wallet:buys:read,wallet:buys:create,wallet:sells:read,wallet:sells:create";

    private const string TransactionsScope =
        "wallet:transactions:send,wallet:transactions:transfer,wallet:addresses:read" +
        ",wallet:addresses:create,wallet:transactions:request&meta[send_limit_amount]=1" +
        "&meta[send_limit_currency]=USD&meta[send_limit_period]=day";

    private const string AuthLinkTemplate =
        "https://www.coinbase.com/oauth/authorize?response_type=code&client_id={0}&redirect_uri={1}&state={2}&scope={3}&account=all";

    internal static class CoinbaseClientIdVersions
    {
        internal const int V1 = 1;
        internal const int V2 = 2;
    }

    public BrokerAuthPromptResponse GetAuthLink(string redirectLink, bool enableTransfers)
    {
        var state = GetNonce();

        string clientId = CoinbaseConfig.ClientId;;

        var redirectUrl = !string.IsNullOrEmpty(redirectLink)
            ? redirectLink
            : CoinbaseConfig.OAuthRedirectLink;

        string scope = enableTransfers ? $"{DefaultScope},{TransactionsScope}" : DefaultScope;

        var link = string.Format(AuthLinkTemplate, clientId, redirectUrl, state, scope);
        return new BrokerAuthPromptResponse()
        {
            Status = BrokerAuthPromptStatus.Redirect,
            LinkToken = link,
            RedirectLink = redirectUrl
        };
    }

    public async Task<BrokerAuthResponse> Authenticate(BrokerAuthRequest request)
    {
        string clientId = CoinbaseConfig.ClientId;;
        string clientSecret = CoinbaseConfig.ClientSecret;

        if (string.IsNullOrEmpty(clientId))
        {
            throw new InvalidOperationException(ClientIdNotSupportedErrorMessage);
        }

        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new InvalidOperationException(ClientSecretNotProvidedErrorMessage);
        }

        var formData = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", request.AuthToken),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("redirect_uri",
                !string.IsNullOrEmpty(request.RedirectLink)
                    ? request.RedirectLink
                    : CoinbaseConfig.OAuthRedirectLink)
        });

        var oAuthResult = await ExecuteOAuthRequest(formData);
        if (!oAuthResult.success)
        {
            return oAuthResult.errorResult;
        }

        var user = await GetCurrentUserInfo(oAuthResult.authResponse.AccessToken);
        var accounts = await GetAccounts(user.Data.Id, oAuthResult.authResponse.AccessToken);

        var response = new BrokerAuthResponse()
        {
            AccessToken = CreateCoinbaseToken(user.Data.Id, oAuthResult.authResponse.AccessToken),
            ExpiresInSeconds = oAuthResult.authResponse.ExpiresIn,
            RefreshToken = CreateRefreshToken(oAuthResult.authResponse.RefreshToken, CoinbaseConfig.ClientIdVersion),
            Status = BrokerAuthStatus.Succeeded
        };

        EnrichWithAccountData(response, user, accounts);

        return response;
    }

    private static void EnrichWithAccountData(BrokerAuthResponse response, UserInfo user,
        IReadOnlyCollection<Account> accounts)
    {
        var cashAccount = GetCashAccount(accounts);
        var nonEmptyAccountSymbols = GetNonEmptyCryptoWallets(accounts)
            .OrderByDescending(x => x.Balance?.Amount)
            .Select(x => x.Currency.Code)
            .Distinct()
            .ToList();

        var accountsName = GetAccountsNameString(nonEmptyAccountSymbols, user);
        var cash = cashAccount?.Balance?.Amount;

        response.Account = new BrokerAccount()
        {
            BuyingPower = cash,
            Cash = cash,
            // In Coinbase, each wallet (BTC, ETH and so on) is a separate account, connecting them separately 
            // would be not convenient for users, so we will connect all at once (oauth app is configured this way)
            // and use user id as account id
            AccountId = user.Data.Id,
            AccountName = accountsName
        };
    }

    // Make account name. If there are currencies available, concatenate them so that the name
    // looks like "ETH, BTC, DASH wallets". Otherwise, use returned from API user's name, e.g. "Username's wallets"
    private static string GetAccountsNameString(IReadOnlyCollection<string> nonEmptyAccountSymbols, UserInfo user)
    {
        var accountsName = string.Format(AccountNameTemplate,
            nonEmptyAccountSymbols.Any() && nonEmptyAccountSymbols.Count <= MaxNumberOfSymbolsToIncludeToAccountName
                ? string.Join(", ", nonEmptyAccountSymbols)
                : user.Data.Name + "'s");
        return accountsName;
    }

    public string GetAccountId(string accessToken)
    {
        var authData = GetAuthData(accessToken);
        return authData.userId;
    }

    public async Task<BrokerAuthResponse> RefreshAccessToken(string accessToken)
    {
        var tokenModel = GetRefreshTokenData(accessToken);

        string clientId = CoinbaseConfig.ClientId;
        string clientSecret = CoinbaseConfig.ClientSecret;
        bool? requiresReauthentication = false;

        

        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new InvalidOperationException(ClientSecretNotProvidedErrorMessage);
        }

        var formData = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("refresh_token", tokenModel.refreshToken),
        });

        var oAuthResult = await ExecuteOAuthRequest(formData);
        if (!oAuthResult.success)
        {
            return oAuthResult.errorResult;
        }

        var user = await GetCurrentUserInfo(oAuthResult.authResponse.AccessToken);
        var accounts = await GetAccounts(user.Data.Id, oAuthResult.authResponse.AccessToken);

        var response = new BrokerAuthResponse()
        {
            AccessToken = CreateCoinbaseToken(user.Data.Id, oAuthResult.authResponse.AccessToken),
            ExpiresInSeconds = oAuthResult.authResponse.ExpiresIn,
            RefreshToken = CreateRefreshToken(oAuthResult.authResponse.RefreshToken, tokenModel.clientIdVersion),
            Status = BrokerAuthStatus.Succeeded,
            RequiresReauthentication = requiresReauthentication
        };

        EnrichWithAccountData(response, user, accounts);

        return response;
    }

    public async Task<string> GetAccountName(string accessToken)
    {
        var tokenModel = GetAuthData(accessToken);
        var user = await GetCurrentUserInfo(tokenModel.accessToken);
        var accounts = await GetAccounts(tokenModel.userId, tokenModel.accessToken);
        var nonEmptyAccountSymbols =
            GetNonEmptyCryptoWallets(accounts).Select(x => x.Currency.Code).Distinct().ToList();
        return GetAccountsNameString(nonEmptyAccountSymbols, user);
    }

    private static string CreateRefreshToken(string refreshToken, int? clientIdVersion)
    {
        return clientIdVersion.HasValue ? $"{refreshToken}|{clientIdVersion}" : refreshToken;
    }

    private static (string refreshToken, int? clientIdVersion) GetRefreshTokenData(string token)
    {
        var values = token.Split("|");
        if (values.Length == 2)
        {
            var parseResult = int.TryParse(values[1], out var version);
            if (!parseResult)
            {
                throw new InvalidOperationException("Coinbase token data is corrupted.");
            }

            return (values[0], version);
        }

        return (values[0], null);
    }

    private async Task<(bool success, AuthResponse authResponse, BrokerAuthResponse errorResult)> ExecuteOAuthRequest(
        FormUrlEncodedContent formData)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, OAuthEndpoint)
        {
            Content = formData
        };
    
        var response = await HttpClient.SendAsync(httpRequest);
        var content = await response.Content.ReadAsStringAsync();
    
        if (!response.IsSuccessStatusCode)
        {
            var errorModel = content.TryFromJson<OauthErrorResponse>(JsonSerializerBehaviour.SnakeCase);
            if (!string.IsNullOrEmpty(errorModel?.ErrorDescription))
            {
                {
                    var errorResponse = new BrokerAuthResponse()
                    {
                        Status = BrokerAuthStatus.Failed,
                        ErrorMessage = errorModel.ErrorDescription
                    };
                    return (false, null, errorResponse);
                }
            }
    
            throw BrokerErrorResponseHelper.CreateBrokerException(response.StatusCode, string.Empty, content,
                BrokerOperationNames.Authenticate, BrokerType.Coinbase.ToString(), Logger);
        }
    
    
        var result = content.FromJson<AuthResponse>(JsonSerializerBehaviour.SnakeCase);
        return (true, result, null);
    }
    
    private async Task<UserInfo> GetCurrentUserInfo(string accessToken)
        => await Execute<UserInfo>(UserPath, HttpMethod.Get, BrokerOperationNames.GetUserInfo, accessToken);


    private static string CreateCoinbaseToken(string userId, string accessToken)
        => $"{userId}|{accessToken}";
    
    
    ///TODO: Refactor to OAuthHelper class
    public string GetNonce(byte length = 16)
    {
        string Digit = "1234567890";
        string Lower = "abcdefghijklmnopqrstuvwxyz";
        object _lock = new object();
        Random _random = new Random();
        
        string chars = (Lower + Digit);

        char[] nonce = new char[length];
        lock (_lock)
        {
            for (var i = 0; i < nonce.Length; i++)
            {
                nonce[i] = chars[_random.Next(0, chars.Length)];
            }
        }
        return new string(nonce);
    }
}