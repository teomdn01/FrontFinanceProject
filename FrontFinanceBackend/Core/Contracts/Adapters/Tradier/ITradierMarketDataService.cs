using Core.Models.MarketData;

namespace Core.Contracts.Adapters.Tradier;

public interface ITradierMarketDataService
{
    Task<StockMarketDataResponse> GetMarketData(List<string> tickers, string token);
}