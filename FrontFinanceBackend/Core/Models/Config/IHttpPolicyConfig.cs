using Org.Front.Core.Contracts.Models.Configuration;

namespace Org.Front.Core.Contracts.Ports.Configuration
{
    public interface IHttpPolicyConfig
    {
        HttpPolicyConfig HttpPolicyConfig { get; set; }
    }
}