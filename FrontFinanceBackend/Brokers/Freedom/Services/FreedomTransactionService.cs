using Brokers.Freedom.Configuration;
using Core.Contracts.Adapters.Freedom;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Brokers.Freedom.Services;

public class FreedomTransactionService : BaseFreedomService, IFreedomTransactionService
{
    public FreedomTransactionService(HttpClient httpClient, IOptions<FreedomConfig> configOptions, ILogger<FreedomTransactionService> logger) : base(httpClient, configOptions, logger)
    {
    }
    
    public Task<string> PlaceOrder(string symbol, string orderType, string secType)
    {
        throw new NotImplementedException();
    }
}