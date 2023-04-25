using System.Net.Http.Headers;
using System.Text;
using Brokers.Coinbase.Models;
using Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core;
using Org.Front.Core.Contracts.Helpers;
using Org.Front.Core.Contracts.Models.Brokers;

// using Microsoft.Extensions.Caching.Distributed;

namespace Brokers.Coinbase.Services
{
    public class BaseCoinbaseService
    {
        protected readonly HttpClient HttpClient;
        protected readonly CoinbaseConfig CoinbaseConfig;
        protected readonly ILogger Logger;
        //protected readonly IEventService EventService;
        protected const string CryptoCurrencyType = "crypto";

        //private readonly IDistributedCache accountsCache;
        private const string AccountsPath = "v2/accounts";
        private const string CacheKey = "coinbase_accounts_{0}";

        public BaseCoinbaseService(HttpClient httpClient,
            IOptions<CoinbaseConfig> configOptions,
            ILogger logger
            //IEventService eventService,
            //DistributedCache accountsCache)
            )
        {
            HttpClient = httpClient;
            Logger = logger;
           // EventService = eventService;
            CoinbaseConfig = configOptions.Value;
           // this.accountsCache = accountsCache;
        }

        protected async Task<IReadOnlyCollection<T>> ExecuteWithPagination<T>(string uri, string accessToken, string operationName)
        {
            var items = new List<T>();
            var nextUri = uri;
            do
            {
                var response = await Execute<BasePaginationResponse<T>>(nextUri, HttpMethod.Get, operationName, accessToken);
                if (response.Data != null && response.Data.Length > 0)
                {
                    items.AddRange(response.Data);
                }

                nextUri = response.Pagination?.NextUri;
            }
            while (!string.IsNullOrEmpty(nextUri));

            return items;
        }

        
        protected async Task<T> Execute<T>(string path, HttpMethod httpMethod, string operationName, string accessToken, object model = null, string mfaCode = null) where T : class
        {
            var request = new HttpRequestMessage(httpMethod, path);
            request.Headers.Add("CB-VERSION", CoinbaseConfig.TargetApiVersion);

            if (!string.IsNullOrEmpty(mfaCode))
            {
                request.Headers.Add("CB-2FA-Token", mfaCode);
            }


            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            if (model != null)
            {
                var modelJson = model.ToJson();
                request.Content = new StringContent(modelJson, Encoding.UTF8, "application/json");
            }

            var response = await HttpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var result = content.FromJson<T>(JsonSerializerBehaviour.SnakeCase);
                    return result;
                }
                catch (Exception e)
                {
                    var errorMessage = "Could not deserialize data from Coinbase";
                    //EventService.TrackTrace($"Coinbase broker problem : {errorMessage}", e.Message);
                    Logger.LogError(e, errorMessage);
                    throw;
                }
            }

            var errorModel = content.TryFromJson<ErrorResponse>(JsonSerializerBehaviour.SnakeCase);

            var parsedMessage = string.Empty;

            if (errorModel?.Errors != null && errorModel.Errors.Any())
            {
                parsedMessage = string.Join(", ", errorModel.Errors.Select(x => x.Message));
            }

            if (response.StatusCode == System.Net.HttpStatusCode.PaymentRequired)
            {
                throw new ArgumentNullException(nameof(mfaCode));
            }
            
            throw BrokerErrorResponseHelper.CreateBrokerException(response.StatusCode, parsedMessage, content, operationName, BrokerType.Coinbase.ToString(), Logger);
        }

        // protected static BrokerOrderStatus MapTransactionStatusToBrokerOrderStatus(TransactionStatus status)
        // {
        //     return status switch
        //     {
        //         TransactionStatus.Pending => BrokerOrderStatus.InProgress,
        //         TransactionStatus.Completed => BrokerOrderStatus.Success,
        //         TransactionStatus.Failed => BrokerOrderStatus.Failed,
        //         TransactionStatus.Expired => BrokerOrderStatus.Failed,
        //         TransactionStatus.Canceled => BrokerOrderStatus.Cancelled,
        //         TransactionStatus.WaitingForSignature => BrokerOrderStatus.InProgress,
        //         TransactionStatus.WaitingForClearing => BrokerOrderStatus.InProgress,
        //         TransactionStatus.Unknown => BrokerOrderStatus.Unknown,
        //         _ => BrokerOrderStatus.Unknown
        //     };
        // }

        // protected static BrokerOrderStatus MapOrderStatusToBrokerOrderStatus(OrderStatus status)
        // {
        //     return status switch
        //     {
        //         OrderStatus.Unknown => BrokerOrderStatus.Unknown,
        //         OrderStatus.Created => BrokerOrderStatus.InProgress,
        //         OrderStatus.Completed => BrokerOrderStatus.Success,
        //         OrderStatus.Canceled => BrokerOrderStatus.Cancelled,
        //         _ => BrokerOrderStatus.Unknown
        //     };
        // }
        //
        // protected static BrokerOrderType MapTransactionTypeToOrderType(TransactionType type)
        // {
        //     return type switch
        //     {
        //         TransactionType.Buy => BrokerOrderType.Buy,
        //         TransactionType.Sell => BrokerOrderType.Sell,
        //         _ => BrokerOrderType.Unknown
        //     };
        // }

        protected static IEnumerable<Account> GetNonEmptyCryptoWallets(IReadOnlyCollection<Account> accounts)
        {
            return accounts.Where(x => x.Currency.Type == "crypto" && x.Balance.Amount > 0);
        }

        protected static Account GetCashAccount(IReadOnlyCollection<Account> accounts)
        {
            var cashAccount = accounts.FirstOrDefault(x => x.AccountType == AccountType.Fiat && x.Currency.Code == "USD");
            return cashAccount;
        }

        protected async Task<IReadOnlyCollection<Account>> GetAccounts(string userId, string accessToken)
        {
           // var cacheKey = string.Format(CacheKey, userId);
            // var cachedData = await accountsCache.GetObjectAsync<List<Account>>(cacheKey);
            // if (cachedData != null)
            // {
            //     return cachedData;
            // }

            var accounts = (await ExecuteWithPagination<Account>(AccountsPath, accessToken, BrokerOperationNames.GetAccounts))
                .ToList();

            // await accountsCache.SetObjectAsync(
            //     cacheKey,
            //     new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) }
            //     , accounts);

            return accounts;
        }

        protected static (string userId, string accessToken) GetAuthData(string token)
        {
            var values = token.Split("|");

            if (values.Length != 2)
            {
                var errorMessage = "Coinbase token data is corrupted";
                throw new InvalidOperationException(errorMessage);
            }

            return (values[0], values[1]);
        }

        protected static List<Account> GetEverUpdatedCryptoAccounts(IReadOnlyCollection<Account> accounts)
        {
            // We don't know which accounts had transactions before.
            // For example, account can have 0 positions (all were sold), but were used before
            // which affects performance. So we need to perform this call against all possible accounts (wallets).
            // But if account creation date equals account update date, it means that it has not been used yet.

            var cryptoAccounts =
                accounts.Where(x => x.Currency.Type == CryptoCurrencyType
                                    && x.CreatedAt.HasValue
                                    && x.UpdatedAt.HasValue
                                    && x.CreatedAt != x.UpdatedAt).ToList();
            return cryptoAccounts;
        }

    }
}
