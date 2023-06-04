using Brokers.Tradier.Configuration;
using Brokers.Tradier.Models;
using Core.Contracts.Adapters.Freedom;
using Core.Contracts.Adapters.Tradier;
using Core.Models;
using Core.Models.MarketData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Brokers.Tradier.Services;

public class TradierMarketDataService : BaseTradierService, ITradierMarketDataService
{
    
    public TradierMarketDataService(HttpClient httpClient, IOptions<TradierConfig> configOptions, ILogger<TradierMarketDataService> logger) : base(httpClient, configOptions, logger)
    {
    }
    
    public async Task<StockMarketDataResponse> GetMarketData(List<string> tickers, string token)
    {
        string queryParams = string.Join(",", tickers);
        string url = $"markets/quotes?symbols={queryParams}";
        var response = await Execute<GetMarketQuotesResponse>(url, HttpMethod.Get, token, "Get market data");

        var result = new StockMarketDataResponse()
        {
            Status = BrokerRequestStatus.Succeeded,
            Data = new List<StockMarketData>()
        };
        response.Quotes.Quote.ForEach(q => result.Data.Add(new StockMarketData()
        {
            Symbol = q.Symbol,
            Name = q.Description,
            Currency = "USD",
            Exchange = q.Exch,
            MarketPrice = q.Close ?? q.Last,
            LastTradeTimestamp = q.TradeDate
        }));

        return result;
    }
}