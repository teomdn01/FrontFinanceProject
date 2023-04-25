using Core.Models.MarketData;

namespace Core.Contracts.Adapters.InteractiveBrokers;

public interface IInteractiveBrokersTransactionService
{
    Task<List<StockMarketDataResponse>> GetMarketData(List<string> symbols);
    Task<string> PlaceOrder(string symbol, string orderType, string secType);
    Task<string> GetPositions();
    Task<string> GetBalance();
}