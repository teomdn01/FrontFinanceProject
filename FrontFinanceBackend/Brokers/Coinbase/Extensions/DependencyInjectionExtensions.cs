// using Brokers.Coinbase;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Org.Front.Adapter.Coinbase.Internal;
// using Brokers.Coinbase.Models;
// using Org.Front.Core.Contracts.Adapters.Coinbase;
// using Org.Front.Core.Contracts.Extensions;
//
// namespace Org.Front.Adapter.Coinbase.Extensions
// {
//     public static class DependencyInjectionExtensions
//     {
//         public static IServiceCollection AddCoinbaseServices(
//             this IServiceCollection services,
//             IConfiguration configuration)
//         {
//             var configSection = configuration.GetSection(nameof(CoinbaseConfig));
//             services.Configure<CoinbaseConfig>(configSection);
//
//             services.AddHttpClientWithPolicies<ICoinbaseAuthService, CoinbaseAuthService, CoinbaseConfig>(configSection);
//             services.AddHttpClientWithPolicies<ICoinbasePortfolioService, CoinbasePortfolioService, CoinbaseConfig>(configSection);
//             services.AddHttpClientWithPolicies<ICoinbaseOrderService, CoinbaseOrderService, CoinbaseConfig>(configSection);
//             services.AddHttpClientWithPolicies<ICoinbaseFeatureService, CoinbaseFeatureService, CoinbaseConfig>(configSection);
//             services.AddHttpClientWithPolicies<ICoinbaseTransactionService, CoinbaseTransactionService, CoinbaseConfig>(configSection);
//             services.AddHttpClientWithPolicies<ICoinbasePriceInfoService, CoinbasePriceInfoService, CoinbaseConfig>(configSection);
//
//             services.AddHttpClientWithPolicies<ICoinbaseTransactionFetchService, CoinbaseTransactionFetchService, CoinbaseConfig>(configSection);
//
//             return services;
//         }
//     }
// }
