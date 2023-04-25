using System;

namespace Org.Front.Core.Contracts.Models.Configuration
{
    public class HttpPolicyConfig
    {
        public RetryPolicyConfig RetryPolicyConfig { get; set; }
        public TimeoutPerTryPolicyConfig TimeoutPerTryPolicyConfig { get; set; }
        public BulkheadPolicyConfig BulkheadPolicyConfig { get; set; }
    }

    public class RetryPolicyConfig
    {
        public bool IsUsed { get; set; }
        public int? RetryCount { get; set; }
        public int? RetryInterval { get; set; }
        public int[] AdditionalStatusCodes { get; set; } = Array.Empty<int>();
    }

    public class TimeoutPerTryPolicyConfig
    {
        public bool IsUsed { get; set; }
        public TimeSpan TimeoutPerTry { get; set; }
    }

    public class BulkheadPolicyConfig
    {
        public bool IsUsed { get; set; }
        public int? MaxParallelization { get; set; }
    }
}