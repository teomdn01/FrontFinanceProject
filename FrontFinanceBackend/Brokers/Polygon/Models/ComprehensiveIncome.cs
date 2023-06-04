using System.Text.Json.Serialization; 
namespace Brokers.Polygon.Models{ 

    public class ComprehensiveIncome
    {
        [JsonPropertyName("comprehensive_income_loss_attributable_to_parent")]
        public ComprehensiveIncomeLossAttributableToParent ComprehensiveIncomeLossAttributableToParent { get; set; }

        [JsonPropertyName("comprehensive_income_loss_attributable_to_noncontrolling_interest")]
        public ComprehensiveIncomeLossAttributableToNoncontrollingInterest ComprehensiveIncomeLossAttributableToNoncontrollingInterest { get; set; }

        [JsonPropertyName("comprehensive_income_loss")]
        public ComprehensiveIncomeLoss ComprehensiveIncomeLoss { get; set; }

        [JsonPropertyName("other_comprehensive_income_loss")]
        public OtherComprehensiveIncomeLoss OtherComprehensiveIncomeLoss { get; set; }

        [JsonPropertyName("other_comprehensive_income_loss_attributable_to_parent")]
        public OtherComprehensiveIncomeLossAttributableToParent OtherComprehensiveIncomeLossAttributableToParent { get; set; }
    }

}