using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Brokers.Freedom.Configuration;
using Brokers.Freedom.Models;
using Core.Contracts.Adapters.Freedom;
using Core.Models;
using Core.Models.MarketData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Brokers.Freedom.Services;

public class FreedomMarketDataService : BaseFreedomService, IFreedomMarketDataService
{
    public FreedomMarketDataService(HttpClient httpClient, IOptions<FreedomConfig> configOptions, ILogger<FreedomMarketDataService> logger) : base(httpClient, configOptions, logger)
    {
    }

    public async Task<StockMarketDataResponse> GetMarketData(List<string> tickers)
    {
        using (var httpClient = new HttpClient())
        {
            var query = new StringBuilder("");
            for (var i = 0; i < tickers.Count - 1; ++i)
            {
                query.Append(tickers[i]);
                query.Append(".US");
                query.Append('+');
            }
            
            query.Append(tickers[tickers.Count - 1]);
            query.Append(".US");

            var request = new HttpRequestMessage(HttpMethod.Get,
                FreedomConfig.PublicBaseEndpoint + $"securities/export?tickers={query}");
            
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var quotes = JsonSerializer.Deserialize<List<GetQuoteResponse>>(content);

                var result = new StockMarketDataResponse();
                result.Data = new List<StockMarketData>();
                quotes.ForEach(quote => result.Data.Add(new StockMarketData()
                {
                    Symbol = quote.code_nm,
                    Name = quote.name,
                    Exchange = quote.codesub_nm,
                    MarketPrice = quote.ClosePrice,
                    Currency = quote.x_curr,
                    LastTradeTimestamp = new DateTimeOffset(DateTime.Parse("2023-04-28T10:30:51"), TimeSpan.Zero).ToUnixTimeSeconds()
                }));
                result.Status = BrokerRequestStatus.Succeeded;
                return result;
            }

            throw new Exception($"FreedomMarketData.GetMarketData() failed with code {response.StatusCode}");
        }
    }
}