using Core.Contracts.Adapters.Freedom;

namespace Brokers.Tradier.Services;

public class TradierTransactionService : ITradierTransactionService
{
    public Task<string> PlaceOrder(string symbol, string orderType, string secType)
    {
        throw new NotImplementedException();
    }
}