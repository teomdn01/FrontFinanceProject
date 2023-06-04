using Alpaca.Markets;
using Brokers.Alpaca.Configuration;
using Brokers.Alpaca.Models;
using Core.Contracts.Adapters.Alpaca;
using Core.Models;
using Core.Models.MarketData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core;
using Org.Front.Core.Contracts.Helpers;
using Org.Front.Core.Contracts.Models.Brokers;
using AuthResponse = Brokers.Coinbase.Models.AuthResponse;

namespace Brokers.Alpaca.Services;

public class AlpacaMarketDataService : BaseAlpacaService, IAlpacaMarketDataService
{
    public AlpacaMarketDataService(IOptions<AlpacaConfig> config, ILogger<AlpacaMarketDataService> logger) : base(
        config, logger)
    {
    }

    public async Task<StockMarketDataResponse> GetMarketData(List<string> symbols, string authToken)
    {
        var token = ParseToken(authToken).Token;
        return await ExecuteAndHandleExceptions(async () =>
        {
            using var client = CreateAlpacaDataClient(token);
            
            var into = DateTime.Now.Subtract(TimeSpan.FromMinutes(15));
            var from = into.Subtract(TimeSpan.FromMinutes(30));

            var quotes = new List<IQuote>();
            foreach (var symbol in symbols)
            {
                var request = new LatestMarketDataRequest(symbol);
                var quote = await client.GetLatestQuoteAsync(request);
                quotes.Add(quote);
            }

            var bars = new List<IBar>();
            foreach (var symbol in symbols)
            {
                var request = new LatestMarketDataRequest(symbol);
                var bar = await client.GetLatestBarAsync(request);
                bars.Add(bar);
            }

            var result = new StockMarketDataResponse()
            {
                Status = BrokerRequestStatus.Succeeded,
                Data = new List<StockMarketData>()
            };
            
            foreach (var bar in bars)
            {
                foreach (var quote in quotes.Where(quote => bar.Symbol == quote.Symbol))
                {
                    result.Data.Add(new StockMarketData()
                    {
                        AskPrice = (double)quote.AskPrice,
                        BidPrice = (double)quote.BidPrice,
                        MarketPrice = (double)bar.Close,
                        LastTradeTimestamp = new DateTimeOffset(quote.TimestampUtc).ToUnixTimeMilliseconds(),
                        Currency = "USD",
                        Exchange = quote.AskExchange,
                        Name = quote.Symbol,
                        Symbol = bar.Symbol
                    });
                }
            }

            return result;
        }, BrokerOperationNames.GetPriceInfo);
    }
}