using Org.Front.Core.Contracts.Models.Configuration;
using Org.Front.Core.Contracts.Ports.Configuration;

namespace Brokers.Trading212.Configuration;

public class Trading212Config : IHttpClientConfig, IHttpPolicyConfig
{
    private static string DefaultBaseEndpoint = "https://live.trading212.com/";
    public Uri BaseEndpoint { get; set; } = new Uri(DefaultBaseEndpoint, UriKind.Absolute);
    public TimeSpan RequestTimeout { get; set; }
    public HttpPolicyConfig HttpPolicyConfig { get; set; }
}