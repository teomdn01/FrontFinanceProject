using Core.Models.MarketData;

namespace Core.Contracts.Adapters.InteractiveBrokers;

public interface IInteractiveBrokersTransactionService
{
    
    Task<string> PlaceOrder(string symbol, string orderType, string secType);

}