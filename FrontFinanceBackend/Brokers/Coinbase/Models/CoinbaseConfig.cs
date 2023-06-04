using Org.Front.Core.Contracts.Models.Configuration;
using Org.Front.Core.Contracts.Ports.Configuration;

namespace Brokers.Coinbase.Models
{
    public class CoinbaseConfig : IHttpClientConfig, IHttpPolicyConfig
    {
        private const string FrontUrl = "https://localhost:3000/";
        private const string DefaultBaseEndpoint = "https://api.coinbase.com/";

        private const int DefaultTimeoutInSeconds = 50;

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientIdV2 { get; set; }
        public string ClientSecretV2 { get; set; }
        public int? ClientIdVersion { get; set; }
        public string OAuthRedirectLink { get; set; } = FrontUrl;
        public string TargetApiVersion { get; set; }

        public HttpPolicyConfig HttpPolicyConfig { get; set; }

        public Uri BaseEndpoint { get; set; } = new Uri(DefaultBaseEndpoint, UriKind.Absolute);

        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(DefaultTimeoutInSeconds);
    }
}
