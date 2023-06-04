using System.Text;
using System.Text.Json;
using Brokers.Freedom.Configuration;
using Brokers.Freedom.Models;
using Core.Contracts.Adapters.Freedom;
using Core.Models;
using Core.Models.AccountInfo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Brokers.Freedom.Services;

public class FreedomAuthService : BaseFreedomService, IFreedomAuthService
{
    public FreedomAuthService(HttpClient httpClient, IOptions<FreedomConfig> configOptions, ILogger<FreedomAuthService> logger) : base(httpClient, configOptions, logger)
    {
    }
    
    public async Task<BrokerAuthResponse> Authenticate(string username, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "check-login-password");

        // Add the necessary headers for x-www-form-urlencoded content
       

        // Build the request body as x-www-form-urlencoded content
        var content = new StringBuilder();
        content.Append($"login={username}&");
        content.Append($"password={password}&");
        content.Append("rememberMe=1");

        // Convert the content to a byte array and set it as the request body
        request.Content = new StringContent(content.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
        
        request.Content.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
        
        var response = await HttpClient.SendAsync(request);
        string resultContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Freedom API login method failed with status code {response.StatusCode}");
        }
        
        LoginResponse loginResult = JsonSerializer.Deserialize<LoginResponse>(resultContent);
        
        return new BrokerAuthResponse()
        {
            AccessToken = loginResult.SID,
            Status = BrokerAuthStatus.Succeeded
        };
    }

    public async Task<BrokerPositions> GetPositions(string apiKey, string apiSecret)
    {
        PublicApiClient client = new PublicApiClient(apiKey, apiSecret, version:2);
        var response = await client.SendRequest("getPositionJson");
        throw new NotImplementedException();
        //return await response.Content.ReadAsStringAsync();
    }

    public async Task<BrokerBalance> GetBalance(string apiKey, string apiSecret)
    {
        PublicApiClient client = new PublicApiClient(apiKey, apiSecret, version:2);
        var response = await client.SendRequest("getPositionJson");
        throw new NotImplementedException();
    }
}