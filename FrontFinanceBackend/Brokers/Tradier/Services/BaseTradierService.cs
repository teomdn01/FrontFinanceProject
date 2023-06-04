using Brokers.Tradier.Configuration;
using Brokers.Tradier.Models;
using Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core;
using Org.Front.Core.Contracts.Helpers;

namespace Brokers.Tradier.Services;

public class BaseTradierService
{
    private const string NullPositionsResponseValue = "{\"positions\":\"null\"}";
    protected readonly HttpClient HttpClient;
    protected readonly TradierConfig TradierConfig;
    protected readonly ILogger Logger;
    
    public BaseTradierService(HttpClient httpClient,
        IOptions<TradierConfig> configOptions,
        ILogger logger)
    {
        HttpClient = httpClient;
        Logger = logger;
        TradierConfig = configOptions.Value;
    }

    protected async Task<GetUserProfileResponse> GetUserInfo(string token)
    {
        return await Execute<GetUserProfileResponse>("user/profile", HttpMethod.Get, token, "Get User Profile");
    }
    
    protected async Task<T> Execute<T>(string path, HttpMethod method, string token, string operationName) where T : class, new()
    {
        Logger.LogInformation($"BaseTradierService starting {operationName} execution");

        var request = new HttpRequestMessage(method, path);
        request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
        request.Headers.Add("Accept", "application/json");
        var response = await HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            //special case for empty portfolio
            if (NullPositionsResponseValue.Equals(content))
            {
                return new T();
            }
            try
            {
                var result = content.FromJson<T>(JsonSerializerBehaviour.SnakeCase);
                return result;
            }
            catch (Exception e)
            {
                var errorMessage = "Could not deserialize data from Tradier";
                Logger.LogError(e, errorMessage);
                throw BrokerErrorResponseHelper.CreateBrokerException(response.StatusCode, content, content, operationName, BrokerType.Tradier.ToString(), Logger);
            }
        }
        throw BrokerErrorResponseHelper.CreateBrokerException(response.StatusCode, content, content, operationName, BrokerType.Tradier.ToString(), Logger);
    }
}