using Brokers.InteractiveBrokers.Configuration;
using Core.Contracts.Adapters.InteractiveBrokers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Brokers.InteractiveBrokers.Services;

public class InteractiveBrokersTransactionService : BaseInteractiveBrokersService, IInteractiveBrokersTransactionService
{
    public InteractiveBrokersTransactionService(HttpClient httpClient, IOptions<InteractiveBrokersConfig> configOptions, ILogger<InteractiveBrokersTransactionService> logger) : base(httpClient, configOptions, logger)
    {
    }

    public Task<string> PlaceOrder(string symbol, string orderType, string secType)
    {
        throw new NotImplementedException();
    }
}