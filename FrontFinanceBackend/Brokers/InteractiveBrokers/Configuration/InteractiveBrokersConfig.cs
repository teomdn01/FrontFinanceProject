using Org.Front.Core.Contracts.Models.Configuration;
using Org.Front.Core.Contracts.Ports.Configuration;

namespace Brokers.InteractiveBrokers.Configuration;

public class InteractiveBrokersConfig : IHttpClientConfig, IHttpPolicyConfig
{
    private static string DefaultBaseEndpoint = "https://localhost:5000/v1/api/";
    public Uri BaseEndpoint { get; set; } = new Uri(DefaultBaseEndpoint, UriKind.Absolute);
    public TimeSpan RequestTimeout { get; set; }
    public HttpPolicyConfig HttpPolicyConfig { get; set; }
}