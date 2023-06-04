using System.Text.Json.Serialization; 
namespace Brokers.Polygon.Models{ 

    public class Financials
    {
        [JsonPropertyName("balance_sheet")]
        public BalanceSheet BalanceSheet { get; set; }

        [JsonPropertyName("cash_flow_statement")]
        public CashFlowStatement CashFlowStatement { get; set; }

        [JsonPropertyName("comprehensive_income")]
        public ComprehensiveIncome ComprehensiveIncome { get; set; }

        [JsonPropertyName("income_statement")]
        public IncomeStatement IncomeStatement { get; set; }
    }

}