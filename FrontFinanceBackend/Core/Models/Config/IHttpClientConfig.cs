using System;

namespace Org.Front.Core.Contracts.Ports.Configuration
{
    public interface IHttpClientConfig
    {
        Uri BaseEndpoint { get; set; }
        TimeSpan RequestTimeout { get; set; }
    }
}