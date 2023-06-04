using Core.Models.MarketData;

namespace Core.Contracts.Adapters.Trading212;

public interface ITrading212MarketDataService
{
    Task<StockMarketDataResponse> GetMarketData(List<string> tickers, string token);
}