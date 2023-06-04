using Brokers.Trading212.Configuration;
using Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core;
using Org.Front.Core.Contracts.Helpers;

namespace Brokers.Trading212.Services;

public class BaseTrading212Service
{
    private const string TraderClientHeaderName = "x-trader-client";
    private const string AcceptEncodingHeaderName = "Accept-Encoding";
    private const string ConnectionHeaderName = "Connection";
    private const string DeviceModelHeaderName = "x-trader-device-model";
    private const string AcceptHeaderName = "Accept";
    private const string UserAgentHeaderName = "User-Agent";
    
    
    private const string TraderClientHeaderValue = "application=MOBILE,os=IOS,type=PHONE,appVersion=587,osVersion=15,dUUID=A26511A4-095B-48B5-9D8D-945332A645DC";
    private const string AcceptEncodingHeaderValue = "gzip, deflate, br";
    private const string ConnectionHeaderValue = "keep-alive";
    private const string DeviceModelHeaderValue = "iPhone";
    private const string AcceptHeaderValue = "*/*";
    private const string UserAgentHeaderValue = "Trading 212/5.161.0 (iPhone; iOS 15.7.3; Scale/2.00)";

    private const string ContentTypeHeaderName = "Content-Type";
    private const string ContentTypeHeaderValue = "application/json";

    protected const string CookieHeaderName = "Cookie";
    protected const string CookieHeaderValue = "TRADING212_SESSION_LIVE={0}";
    
    protected readonly HttpClient HttpClient;
    protected readonly Trading212Config Trading212Config;
    protected readonly ILogger Logger;
    
    public BaseTrading212Service(HttpClient httpClient,
        IOptions<Trading212Config> configOptions,
        ILogger logger)
    {
        HttpClient = httpClient;
        Logger = logger;
        Trading212Config = configOptions.Value; 
    }
    
    
    protected async Task<T> Execute<T>(HttpRequestMessage request, string operationName) where T : class
    {
        Logger.LogInformation($"BaseTrading212Service starting {operationName} execution");
        request.Headers.TryAddWithoutValidation(AcceptHeaderName, AcceptHeaderValue);
        request.Headers.TryAddWithoutValidation(AcceptEncodingHeaderName, AcceptEncodingHeaderValue);
        request.Headers.TryAddWithoutValidation(ConnectionHeaderName, ConnectionHeaderValue);
        request.Headers.TryAddWithoutValidation(TraderClientHeaderName, TraderClientHeaderValue);
        request.Headers.TryAddWithoutValidation(DeviceModelHeaderName, DeviceModelHeaderValue);
        request.Headers.TryAddWithoutValidation(UserAgentHeaderName, UserAgentHeaderValue);
        //request.Headers.TryAddWithoutValidation(ContentTypeHeaderName, ContentTypeHeaderValue);
        
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
                var errorMessage = "Could not deserialize data from Trading212";
                Logger.LogError(e, errorMessage);
                throw BrokerErrorResponseHelper.CreateBrokerException(response.StatusCode, content, content, operationName, BrokerType.Trading212.ToString(), Logger);
            }
        }
        throw BrokerErrorResponseHelper.CreateBrokerException(response.StatusCode, content, content, operationName, BrokerType.Trading212.ToString(), Logger);
    }
}