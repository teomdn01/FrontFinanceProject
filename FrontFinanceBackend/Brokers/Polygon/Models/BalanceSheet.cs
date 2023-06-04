using System.Text.Json.Serialization; 
namespace Brokers.Polygon.Models{ 

    public class BalanceSheet
    {
        [JsonPropertyName("equity_attributable_to_noncontrolling_interest")]
        public EquityAttributableToNoncontrollingInterest EquityAttributableToNoncontrollingInterest { get; set; }

        [JsonPropertyName("equity_attributable_to_parent")]
        public EquityAttributableToParent EquityAttributableToParent { get; set; }

        [JsonPropertyName("noncurrent_assets")]
        public NoncurrentAssets NoncurrentAssets { get; set; }

        [JsonPropertyName("assets")]
        public Assets Assets { get; set; }

        [JsonPropertyName("noncurrent_liabilities")]
        public NoncurrentLiabilities NoncurrentLiabilities { get; set; }

        [JsonPropertyName("current_liabilities")]
        public CurrentLiabilities CurrentLiabilities { get; set; }

        [JsonPropertyName("liabilities")]
        public Liabilities Liabilities { get; set; }

        [JsonPropertyName("fixed_assets")]
        public FixedAssets FixedAssets { get; set; }

        [JsonPropertyName("other_than_fixed_noncurrent_assets")]
        public OtherThanFixedNoncurrentAssets OtherThanFixedNoncurrentAssets { get; set; }

        [JsonPropertyName("liabilities_and_equity")]
        public LiabilitiesAndEquity LiabilitiesAndEquity { get; set; }

        [JsonPropertyName("equity")]
        public Equity Equity { get; set; }

        [JsonPropertyName("current_assets")]
        public CurrentAssets CurrentAssets { get; set; }
    }

}