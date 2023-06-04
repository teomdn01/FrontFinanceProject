using System.Net.Mime;
using System.Text;
using Brokers.Trading212.Configuration;
using Brokers.Trading212.Models;
using Core.Contracts.Adapters.Trading212;
using Core.Models;
using Core.Models.MarketData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core;
using Org.Front.Core.Contracts.Models.Brokers;

namespace Brokers.Trading212.Services;

public class Trading212MarketDataService : BaseTrading212Service, ITrading212MarketDataService
{
    
    public Trading212MarketDataService(HttpClient httpClient, IOptions<Trading212Config> configOptions, ILogger<Trading212MarketDataService> logger) : base(httpClient, configOptions, logger)
    {
    }
    
    public async Task<StockMarketDataResponse> GetMarketData(List<string> tickers, string token)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "charting/v3/candles/close");
        request.Headers.TryAddWithoutValidation(CookieHeaderName, string.Format(CookieHeaderValue, token));

        var body = new GetQuotesRequest()
        {
            Candles = new List<Candle>()
        };
        
        foreach (var ticker in tickers)
        {
            body.Candles.Add(new Candle()
            {
                Period = "FIFTEEN_MINUTES",
                Size = 1,
                Ticker = ticker,
                UseAskPrice = false
            });
        }
        
        request.Content = new StringContent(body.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);
        
        var response = await Execute<List<GetQuotesResponse>>(request, BrokerOperationNames.GetPriceInfo);
        
        var result = new StockMarketDataResponse()
        {
            Status = BrokerRequestStatus.Succeeded,
            Data = new List<StockMarketData>()
        };
        response.ForEach(q => result.Data.Add(new StockMarketData()
        {
            Symbol = q.Request.Ticker,
            Name = q.Request.Ticker,
            Currency = "USD",
            Exchange = "NASDAQ",
            MarketPrice = q.Response.Candles[0][1],
            LastTradeTimestamp = Convert.ToInt64(q.Response.Candles[0][0])
        }));

        return result;
    }
}