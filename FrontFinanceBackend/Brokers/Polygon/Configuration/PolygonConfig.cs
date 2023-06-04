using Org.Front.Core.Contracts.Models.Configuration;
using Org.Front.Core.Contracts.Ports.Configuration;

namespace Brokers.Polygon.Configuration;

public class PolygonConfig : IHttpClientConfig, IHttpPolicyConfig
{
    private static string DefaultBaseEndpoint = "https://api.polygon.io/";
    public Uri BaseEndpoint { get; set; } = new Uri(DefaultBaseEndpoint, UriKind.Absolute);
    public TimeSpan RequestTimeout { get; set; }
    public HttpPolicyConfig HttpPolicyConfig { get; set; }
    public string? ApiKey { get; set; }
}