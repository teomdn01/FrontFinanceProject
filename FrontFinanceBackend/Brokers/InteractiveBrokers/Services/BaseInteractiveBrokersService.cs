using System.Net;
using Brokers.InteractiveBrokers.Configuration;
using Brokers.InteractiveBrokers.Models;
using Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core;
using Org.Front.Core.Contracts.Helpers;

namespace Brokers.InteractiveBrokers.Services;

public class BaseInteractiveBrokersService
{
    protected readonly HttpClient HttpClient;
    protected readonly InteractiveBrokersConfig InteractiveBrokersConfig;
    protected readonly ILogger Logger;
    
    public BaseInteractiveBrokersService(HttpClient httpClient,
        IOptions<InteractiveBrokersConfig> configOptions,
        ILogger logger)
    {
        HttpClient = httpClient;
        Logger = logger;
        InteractiveBrokersConfig = configOptions.Value;
    }

    protected async Task<List<AssetSummary>> GetContractIds(string symbols)
    {
        var assetsInfo = await Execute<AssetsInfo>($"trsrv/stocks?symbols={symbols}" , HttpMethod.Get, "Get Contract IDs");

        var result = new List<AssetSummary>();
        foreach (var kvp in assetsInfo.Data)
        {
            foreach (var asset in kvp.Value)
            {
                asset.Contracts.ForEach(contract => result.Add(new AssetSummary()
                {
                    Symbol = kvp.Key,
                    Name = asset.Name,
                    Contract = contract
                }));
            }
        }

        return result;
    }

    protected async Task<StockHistory> GetStockMarketHistory(int contractId)
    {
        var stockData = await Execute<StockHistory>(
            $"iserver/marketdata/history?conid={contractId}&period=1min&bar=1min", HttpMethod.Get, "Get Stock History");
        return stockData;
    }
    
    protected async Task<string> GetAccounts()
    {
        throw new NotImplementedException();
    }
    
    

    protected async Task<T> Execute<T>(string path, HttpMethod method, string operationName) where T : class
    {
        Logger.LogInformation("BaseInteractiveBrokers execution starting...");

        var request = new HttpRequestMessage(method, path);
        var response = await HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var result = content.FromJson<T>(JsonSerializerBehaviour.SnakeCase);
                return result;
            }
            catch (Exception e)
            {
                var errorMessage = "Could not deserialize data from Interactive Brokers";
                Logger.LogError(e, errorMessage);
                throw;
            }
        }
        throw BrokerErrorResponseHelper.CreateBrokerException(response.StatusCode, content, content, operationName, BrokerType.InteractiveBrokers.ToString(), Logger);
    }
}