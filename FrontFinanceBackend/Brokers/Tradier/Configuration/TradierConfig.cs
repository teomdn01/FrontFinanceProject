using Org.Front.Core.Contracts.Models.Configuration;
using Org.Front.Core.Contracts.Ports.Configuration;

namespace Brokers.Tradier.Configuration;

public class TradierConfig : IHttpClientConfig, IHttpPolicyConfig
{
    private static string DefaultBaseEndpoint = "https://api.tradier.com/v1/";
    public Uri BaseEndpoint { get; set; } = new Uri(DefaultBaseEndpoint, UriKind.Absolute);
    public TimeSpan RequestTimeout { get; set; }
    public HttpPolicyConfig HttpPolicyConfig { get; set; }
}