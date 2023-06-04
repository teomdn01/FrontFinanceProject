using Alpaca.Markets;
using Brokers.Alpaca.Configuration;
using Brokers.Alpaca.Models;
using Core.Contracts.Adapters.Alpaca;
using Core.Models;
using Core.Models.AccountInfo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core;
using Org.Front.Core.Contracts.Helpers;
using Org.Front.Core.Contracts.Models.Brokers;

namespace Brokers.Alpaca.Services;

public class AlpacaAuthService : BaseAlpacaService, IAlpacaAuthService
{
    private static readonly string authorizeLinkTemplate =
        "https://app.alpaca.markets/oauth/authorize?response_type=code&client_id={0}&redirect_uri={1}&scope=account:write%20trading";

    private const string GetAccessTokenEndpoint = "https://api.alpaca.markets/oauth/token";

    private readonly HttpClient httpClient;
    private readonly ILogger<AlpacaAuthService> logger;
    
    public AlpacaAuthService(IOptions<AlpacaConfig> config, ILogger<AlpacaAuthService> logger, HttpClient httpClient) : base(config, logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }
    
   public BrokerAuthPromptResponse GetLinkToken(string redirectLink)
        {
            var redirectUrl = !string.IsNullOrEmpty(redirectLink)
                ? redirectLink
                : AlpacaConfig.AlpacaOAuthRedirectLink;

            var link = string.Format(authorizeLinkTemplate, AlpacaConfig.AlpacaClientId, redirectUrl);
            return new BrokerAuthPromptResponse()
            {
                Status = BrokerAuthPromptStatus.Redirect,
                LinkToken = link,
                RedirectLink = redirectUrl
            };
        }

        public async Task<BrokerAuthResponse> Authenticate(BrokerAuthRequest brokerAuthRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, GetAccessTokenEndpoint)
            {
                Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", brokerAuthRequest.AuthToken),
                    new KeyValuePair<string, string>("client_id", AlpacaConfig.AlpacaClientId),
                    new KeyValuePair<string, string>("client_secret", AlpacaConfig.AlpacaClientSecret),
                    new KeyValuePair<string, string>("redirect_uri",
                        !string.IsNullOrEmpty(brokerAuthRequest.RedirectLink)
                            ? brokerAuthRequest.RedirectLink
                            : AlpacaConfig.AlpacaOAuthRedirectLink)
                })
            };

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorModel = content.TryFromJson<ErrorModel>();
                throw BrokerErrorResponseHelper.CreateBrokerException(response.StatusCode, errorModel?.Message, content,
                    BrokerOperationNames.Authenticate, "Alpaca", logger);
            }

            var result = content.FromJson<AuthResponse>();

            var account = await GetAccount(result.AccessToken);

            var tokenModel = new TokenModel()
            {
                AccountId = account.AccountId.ToString(),
                Token = result.AccessToken
            };

            return new BrokerAuthResponse()
            {
                AccessToken = CreateTokenWithAccountId(tokenModel),
                Status = BrokerAuthStatus.Succeeded,
                Account = new BrokerAccount()
                {
                    AccountId = tokenModel.AccountId,
                    BuyingPower = account.BuyingPower,
                    Cash = account.TradableCash,
                    AccountName = account.AccountNumber
                }
            };
        }

        public string GetAccountId(string accessToken)
        {
            return accessToken.Contains("|") ? accessToken.Split("|")[1] : null;
        }

        public async Task<string> GetAccountName(string accessToken)
        {
            var token = ParseToken(accessToken).Token;
            var account = await GetAccount(token);
            return account.AccountNumber;
        }

    public async Task<BrokerPositions> GetPositions(string authToken)
    {
        var token = ParseToken(authToken).Token;
        return await ExecuteAndHandleExceptions(async () =>
        {
            using var client = CreateAlpacaClient(token);

            var positions = await client.ListPositionsAsync();

            var stockPositions = positions
                .Where(p => p.AssetClass == AssetClass.UsEquity)
                .Select(x => new Position()
                {
                    Amount = x.Quantity,
                    CostBasis = x.CostBasis,
                    Symbol = x.Symbol
                }).ToList();

            var cryptoPositions = positions
                .Where(p => p.AssetClass == AssetClass.Crypto)
                .Select(x => new Position()
                {
                    Amount = x.Quantity,
                    CostBasis = x.CostBasis,
                    Symbol = x.Symbol[..^3]
                }).ToList();
            
            stockPositions.AddRange(cryptoPositions);
            
            return (new BrokerPositions()
            {
                Positions = stockPositions,
                Status = BrokerRequestStatus.Succeeded
            });
        }, BrokerOperationNames.GetPortfolio);
    }

    public async Task<BrokerBalance> GetBalance(string accessToken)
    {
        var token = ParseToken(accessToken).Token;

        var account = await GetAccount(token);

        return new BrokerBalance()
        {
            Status = BrokerRequestStatus.Succeeded,
            BuyingPower = account.BuyingPower,
            Cash = (double)account.TradableCash
        };
    }
}