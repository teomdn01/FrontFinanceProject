using Core.Models.MarketData;

namespace Core.Contracts.Adapters.InteractiveBrokers;

public interface IInteractiveBrokersMarketDataService
{
    Task<StockMarketDataResponse> GetMarketData(List<string> symbols);   
}