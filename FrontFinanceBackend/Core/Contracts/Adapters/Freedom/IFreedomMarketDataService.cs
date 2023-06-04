using Core.Models.MarketData;

namespace Core.Contracts.Adapters.Freedom;

public interface IFreedomMarketDataService
{
    Task<StockMarketDataResponse> GetMarketData(List<string> tickers);
}