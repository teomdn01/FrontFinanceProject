using System.Text.Json.Serialization; 
namespace Brokers.Polygon.Models{ 

    public class CashFlowStatement
    {
        [JsonPropertyName("net_cash_flow_from_investing_activities")]
        public NetCashFlowFromInvestingActivities NetCashFlowFromInvestingActivities { get; set; }

        [JsonPropertyName("net_cash_flow_from_operating_activities")]
        public NetCashFlowFromOperatingActivities NetCashFlowFromOperatingActivities { get; set; }

        [JsonPropertyName("net_cash_flow")]
        public NetCashFlow NetCashFlow { get; set; }

        [JsonPropertyName("net_cash_flow_continuing")]
        public NetCashFlowContinuing NetCashFlowContinuing { get; set; }

        [JsonPropertyName("net_cash_flow_from_operating_activities_continuing")]
        public NetCashFlowFromOperatingActivitiesContinuing NetCashFlowFromOperatingActivitiesContinuing { get; set; }

        [JsonPropertyName("net_cash_flow_from_financing_activities")]
        public NetCashFlowFromFinancingActivities NetCashFlowFromFinancingActivities { get; set; }

        [JsonPropertyName("net_cash_flow_from_investing_activities_continuing")]
        public NetCashFlowFromInvestingActivitiesContinuing NetCashFlowFromInvestingActivitiesContinuing { get; set; }

        [JsonPropertyName("net_cash_flow_from_financing_activities_continuing")]
        public NetCashFlowFromFinancingActivitiesContinuing NetCashFlowFromFinancingActivitiesContinuing { get; set; }
    }

}