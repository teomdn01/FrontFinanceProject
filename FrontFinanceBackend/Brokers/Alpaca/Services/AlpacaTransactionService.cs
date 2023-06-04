using Brokers.Alpaca.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core.Contracts.Adapters.Alpaca;

namespace Brokers.Alpaca.Services;

public class AlpacaTransactionService : BaseAlpacaService, IAlpacaTransactionService
{
    public AlpacaTransactionService(IOptions<AlpacaConfig> config, ILogger<AlpacaTransactionService> logger) : base(config, logger)
    {
    }
}