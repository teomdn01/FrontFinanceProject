using Brokers.Freedom.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Brokers.Freedom.Services;

public class BaseFreedomService
{
    protected readonly HttpClient HttpClient;
    protected readonly FreedomConfig FreedomConfig;
    protected readonly ILogger Logger;
    
    public BaseFreedomService(HttpClient httpClient,
        IOptions<FreedomConfig> configOptions,
        ILogger logger)
    {
        HttpClient = httpClient;
        Logger = logger;
        FreedomConfig = configOptions.Value;
    }
}