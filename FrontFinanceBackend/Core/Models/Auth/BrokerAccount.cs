using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Org.Front.Core.Contracts.Models.Brokers
{
    public class BrokerAccount
    {
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }

        [JsonPropertyName("accountName")]
        public string AccountName { get; set; }

        /// <summary>
        /// Buying power of the account. Typically consists of cash plus available margin.
        /// For non-margin accounts fund contains cash only
        /// </summary>
        [JsonPropertyName("fund")]
        public decimal? BuyingPower { get; set; }

        /// <summary>
        /// Cash balance in USD
        /// </summary>
        [JsonPropertyName("cash")]
        public decimal? Cash { get; set; }

        /// <summary>
        /// Indicates if this account was already connected by the current user and device.
        /// Can be null.
        /// </summary>
        [JsonPropertyName("isReconnected")]
        public bool? IsReconnected { get; set; }

        /// <summary>
        /// The list of all asset balances of account 
        /// </summary>
        [JsonPropertyName("balances")]
        public IReadOnlyCollection<BrokerFiatBalance> Balances { get; set; }
    }
}
