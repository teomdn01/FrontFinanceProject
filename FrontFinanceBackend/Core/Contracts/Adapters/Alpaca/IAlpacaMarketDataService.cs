using Core.Models.MarketData;

namespace Core.Contracts.Adapters.Alpaca
{
    public interface IAlpacaMarketDataService
    {
        Task<StockMarketDataResponse> GetMarketData(List<string> symbols, string token);
    }
}
