using Brokers.InteractiveBrokers.Configuration;
using Core.Contracts.Adapters.InteractiveBrokers;
using Core.Models;
using Core.Models.MarketData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Brokers.InteractiveBrokers.Services;

public class InteractiveBrokersMarketDataService : BaseInteractiveBrokersService, IInteractiveBrokersMarketDataService
{
    public InteractiveBrokersMarketDataService(HttpClient httpClient, IOptions<InteractiveBrokersConfig> configOptions, ILogger<InteractiveBrokersMarketDataService> logger) : base(httpClient, configOptions, logger)
    {
    }

    public async Task<StockMarketDataResponse> GetMarketData(List<string> symbols)
    {
        var querySymbols = string.Join(",", symbols);
        var assets = await GetContractIds(querySymbols);
        var result = new StockMarketDataResponse();
        result.Data = new List<StockMarketData>();
        foreach (var asset in assets)
        {
            try
            {
                var stockHistory = await GetStockMarketHistory(asset.Contract.ConId);
                if (stockHistory.Data == null || stockHistory.Data.Count == 0)
                {
                    asset.MarketPrice = 0.0;
                    asset.Error = "Could not find asset trading history";
                }
                else
                {
                    asset.MarketPrice = stockHistory.Data[0].c;
                    asset.LastTradeTimestamp = stockHistory.Data[0].t;
                }
            }
            catch (Exception e)
            {
                asset.MarketPrice = 0.0;
                asset.Error = e.Message;
            }
            
            result.Data.Add(new StockMarketData()
            {
                Symbol = asset.Symbol,
                Name = asset.Name,
                Exchange = asset.Contract.ConId.ToString(),
                MarketPrice = asset.MarketPrice,
                LastTradeTimestamp = asset.LastTradeTimestamp,
                Error = asset.Error
            });
        }

        result.Status = BrokerRequestStatus.Succeeded;
        return result;
    }
}