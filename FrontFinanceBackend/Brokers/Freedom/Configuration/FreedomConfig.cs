using Org.Front.Core.Contracts.Models.Configuration;
using Org.Front.Core.Contracts.Ports.Configuration;

namespace Brokers.Freedom.Configuration;

public class FreedomConfig : IHttpClientConfig, IHttpPolicyConfig
{
    private static string DefaultBaseEndpoint = "https://tradernet.ru/api/";
    private static string DefaultPublicBaseEndpoint = "https://freedom24.com/";
    public Uri BaseEndpoint { get; set; } = new Uri(DefaultBaseEndpoint, UriKind.Absolute);
    public Uri PublicBaseEndpoint { get; set; } = new Uri(DefaultPublicBaseEndpoint, UriKind.Absolute);
    public TimeSpan RequestTimeout { get; set; }
    public HttpPolicyConfig HttpPolicyConfig { get; set; }
}