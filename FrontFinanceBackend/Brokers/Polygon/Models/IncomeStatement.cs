using System.Text.Json.Serialization; 
namespace Brokers.Polygon.Models{ 

    public class IncomeStatement
    {
        [JsonPropertyName("net_income_loss_available_to_common_stockholders_basic")]
        public NetIncomeLossAvailableToCommonStockholdersBasic NetIncomeLossAvailableToCommonStockholdersBasic { get; set; }

        [JsonPropertyName("operating_expenses")]
        public OperatingExpenses OperatingExpenses { get; set; }

        [JsonPropertyName("operating_income_loss")]
        public OperatingIncomeLoss OperatingIncomeLoss { get; set; }

        [JsonPropertyName("costs_and_expenses")]
        public CostsAndExpenses CostsAndExpenses { get; set; }

        [JsonPropertyName("basic_earnings_per_share")]
        public BasicEarningsPerShare BasicEarningsPerShare { get; set; }

        [JsonPropertyName("interest_expense_operating")]
        public InterestExpenseOperating InterestExpenseOperating { get; set; }

        [JsonPropertyName("revenues")]
        public Revenues Revenues { get; set; }

        [JsonPropertyName("cost_of_revenue")]
        public CostOfRevenue CostOfRevenue { get; set; }

        [JsonPropertyName("benefits_costs_expenses")]
        public BenefitsCostsExpenses BenefitsCostsExpenses { get; set; }

        [JsonPropertyName("net_income_loss")]
        public NetIncomeLoss NetIncomeLoss { get; set; }

        [JsonPropertyName("income_tax_expense_benefit")]
        public IncomeTaxExpenseBenefit IncomeTaxExpenseBenefit { get; set; }

        [JsonPropertyName("participating_securities_distributed_and_undistributed_earnings_loss_basic")]
        public ParticipatingSecuritiesDistributedAndUndistributedEarningsLossBasic ParticipatingSecuritiesDistributedAndUndistributedEarningsLossBasic { get; set; }

        [JsonPropertyName("income_loss_from_continuing_operations_before_tax")]
        public IncomeLossFromContinuingOperationsBeforeTax IncomeLossFromContinuingOperationsBeforeTax { get; set; }

        [JsonPropertyName("income_loss_from_continuing_operations_after_tax")]
        public IncomeLossFromContinuingOperationsAfterTax IncomeLossFromContinuingOperationsAfterTax { get; set; }

        [JsonPropertyName("net_income_loss_attributable_to_noncontrolling_interest")]
        public NetIncomeLossAttributableToNoncontrollingInterest NetIncomeLossAttributableToNoncontrollingInterest { get; set; }

        [JsonPropertyName("diluted_earnings_per_share")]
        public DilutedEarningsPerShare DilutedEarningsPerShare { get; set; }

        [JsonPropertyName("preferred_stock_dividends_and_other_adjustments")]
        public PreferredStockDividendsAndOtherAdjustments PreferredStockDividendsAndOtherAdjustments { get; set; }

        [JsonPropertyName("gross_profit")]
        public GrossProfit GrossProfit { get; set; }

        [JsonPropertyName("net_income_loss_attributable_to_parent")]
        public NetIncomeLossAttributableToParent NetIncomeLossAttributableToParent { get; set; }

        [JsonPropertyName("nonoperating_income_loss")]
        public NonoperatingIncomeLoss NonoperatingIncomeLoss { get; set; }

        [JsonPropertyName("income_tax_expense_benefit_deferred")]
        public IncomeTaxExpenseBenefitDeferred IncomeTaxExpenseBenefitDeferred { get; set; }
    }

}