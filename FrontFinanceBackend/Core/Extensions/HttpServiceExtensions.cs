using System;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Org.Front.Core.Contracts.Ports.Configuration;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace Org.Front.Core.Contracts.Extensions
{
    public static class HttpServiceExtensions
    {
        private const int DefaultRetryInterval = 1;

        public static IHttpClientBuilder AddHttpClientWithPolicies<TContract, TImplementation, TConfig>(
          this IServiceCollection services,
          IConfigurationSection configurationSection,
          string name = null)
          where TContract : class
          where TImplementation : class, TContract
          where TConfig : class, IHttpClientConfig, IHttpPolicyConfig, new()
        {
            var clientConfig = configurationSection.Get<TConfig>() ?? new TConfig();

            var httpBuilder = (!string.IsNullOrEmpty(name)
                ? services.AddHttpClient<TContract, TImplementation>(name)
                : services.AddHttpClient<TContract, TImplementation>())
                    .ConfigureHttpClient((sp, httpClient) =>
                    {
                        if (clientConfig.BaseEndpoint != null)
                        {
                            httpClient.BaseAddress = clientConfig.BaseEndpoint;
                        }

                        if (clientConfig.RequestTimeout.TotalSeconds > 0)
                        {
                            httpClient.Timeout = clientConfig.RequestTimeout;
                        }
                    });

            if (clientConfig.HttpPolicyConfig != null)
            {
                var retryPolicyConfig = clientConfig.HttpPolicyConfig.RetryPolicyConfig;
                if (retryPolicyConfig.IsUsed && retryPolicyConfig.RetryCount != null && retryPolicyConfig.RetryCount > 0)
                {
                    IAsyncPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .OrResult(
                            msg => (retryPolicyConfig.AdditionalStatusCodes ?? Array.Empty<int>())
                                .Contains((int)msg.StatusCode))
                        .Or<SocketException>()
                        .Or<TimeoutRejectedException>()
                        .WaitAndRetryAsync(
                            retryPolicyConfig.RetryCount.Value,
                            retryAttempt => TimeSpan.FromSeconds(retryPolicyConfig.RetryInterval ?? DefaultRetryInterval));

                    var noOpPolicyHandler = Policy
                        .NoOpAsync()
                        .AsAsyncPolicy<HttpResponseMessage>();

                    httpBuilder = httpBuilder.AddPolicyHandler(
                        request => request.Method == HttpMethod.Get
                            ? retryPolicy
                            : noOpPolicyHandler);
                }


                var timeoutPerTryPolicyConfig = clientConfig.HttpPolicyConfig.TimeoutPerTryPolicyConfig;
                if (timeoutPerTryPolicyConfig != null)
                {
                    if (timeoutPerTryPolicyConfig.IsUsed && timeoutPerTryPolicyConfig.TimeoutPerTry.TotalSeconds > 0)
                    {
                        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
                            Convert.ToInt32(timeoutPerTryPolicyConfig.TimeoutPerTry.TotalSeconds));
                        // We place the timeoutPolicy inside the retryPolicy, to make it time out each try.
                        httpBuilder.AddPolicyHandler(timeoutPolicy);
                    }
                }

                var bulkheadPolicyConfig = clientConfig.HttpPolicyConfig.BulkheadPolicyConfig;
                if (bulkheadPolicyConfig != null)
                {
                    if (bulkheadPolicyConfig.IsUsed
                        && bulkheadPolicyConfig.MaxParallelization.HasValue
                        && bulkheadPolicyConfig.MaxParallelization > 0)
                    {
                        // Setup bulkhead policy. We use configured MaxParallelization value and int.MaxValue as maximum amount of
                        // queuing actions (to make sure Polly does not throw exceptions if a queue is too small)
                        var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(
                            bulkheadPolicyConfig.MaxParallelization.Value,
                            Int32.MaxValue);

                        httpBuilder.AddPolicyHandler(bulkheadPolicy);
                    }
                }
            }

            return httpBuilder;
        }
    }
}
