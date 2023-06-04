namespace Core.Models.Finances;

public class FinancialStatementsAnalysis
{
    public FinancialStatementsAnalysis()
    {
        grossProfitMargin = new GrossProfitMargin();
        interestExpenseToOperatingIncomeRatio = new InterestExpenseToOperatingIncomeRatio();
        netIncomeToTotalRevenueRatio = new NetIncomeToTotalRevenueRatio();
        perShareEarningsVolatilityFactor = new PerShareEarningsVolatilityFactor();
        operationMarginRatio = new OperationMarginRatio();
        returnOnAssetsRatio = new ReturnOnAssetsRatio();
        currentAssetsLiabilitiesRatio = new CurrentAssetsLiabilitiesRatio();
        debtToShareholdersEquityRatio = new DebtToShareholdersEquityRatio();
        longTermDebt = new LongTermDebt();
        preferredStock = new PreferredStock();
        returnOnShareHoldersEquityRatio = new ReturnOnShareHoldersEquityRatio();
        netOfIssuanceStock = new NetOfIssuanceStock();
        investingToOperatingCashFlowRatio = new InvestingToOperatingCashFlowRatio();
    }

    /// <summary>
    /// Desired value: > 0.4
    /// </summary>
    ///
    public GrossProfitMargin grossProfitMargin { get; set; }
    public class GrossProfitMargin
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 4;
        public double LowerBound { get; set; } = 0.4d;
        public string ToolTip { get; set; } = "Desired value: > 0.4";
    }
    
    
    /// <summary>
    /// Desired value: >5 (safe) (10 safe enough) (30 doesn't matter, too big anyway)
    /// </summary>
    public InterestExpenseToOperatingIncomeRatio interestExpenseToOperatingIncomeRatio { get; set; }
    public class InterestExpenseToOperatingIncomeRatio
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 3;
        public double LowerBound { get; set; } = 5d;
        public string ToolTip { get; set; } = "Desired value: >5 (safe) (10 safe enough)";
    }
    
    /// <summary>
    /// Desired Value: >20% good, <10% bad, in between - grey zone
    /// </summary>
    public NetIncomeToTotalRevenueRatio netIncomeToTotalRevenueRatio { get; set; }
    public class NetIncomeToTotalRevenueRatio
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 4;
        public double LowerBound { get; set; } = 0.1;
        public double UpperBound { get; set; } = 0.2;
        public string ToolTip { get; set; } = "Desired Value: >20% good, <10% bad, in between - grey zone";
    }
    /// <summary>
    /// Searching for an ascending trend over the past 10 years.
    /// Volatility increases every time we cannot identify an ascending trend comparing to previous year
    /// Desired value: 0 (no volatility, only ascending trends) (values from [0,1])
    /// </summary>
    public PerShareEarningsVolatilityFactor perShareEarningsVolatilityFactor { get; set; }
    public class PerShareEarningsVolatilityFactor
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 2;
        public double TargetValue { get; set; } = 0;
        public string ToolTip { get; set; } = "Desired value: 0 (no volatility, only ascending trends) (values from [0,1])";
    }

    /// <summary>
    /// Desired value: > 30%
    /// </summary>
    public OperationMarginRatio operationMarginRatio { get; set; }
    public class OperationMarginRatio
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 4;
        public double LowerBound { get; set; } = 0.3d;
        public string ToolTip { get; set; } = "Desired value: > 30%";
    }

    /// <summary>
    /// Desired value: 10-40% - huge variant, bigger does not necessarily mean better, Accounting book says > 6%
    /// </summary>
    public ReturnOnAssetsRatio returnOnAssetsRatio { get; set; }
    public class ReturnOnAssetsRatio
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 3;
        public double LowerBound { get; set; } = 0.1d;
        public string ToolTip { get; set; } = "Desired value: 10-40% - huge variant, bigger does not necessarily mean better, but it's good to have it above 10%";
    }
    
    
    /// <summary>
    /// Desired value: > 1, around 1,5. <5 (5 already means BAD money management) 
    /// </summary>
    public CurrentAssetsLiabilitiesRatio currentAssetsLiabilitiesRatio { get; set; }
    public class CurrentAssetsLiabilitiesRatio
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 3;
        public double LowerBound { get; set; } = 1;
        public double DesiredValue { get; set; } = 1.5;
        public double UpperBound { get; set; } = 5;
        public string ToolTip { get; set; } = "Desired value: > 1, around 1,5. <5 (5 already means BAD money management)";
    }

    /// <summary>
    /// Desired value: < 0.8. Accounting book: < 0.5
    /// </summary>
    public DebtToShareholdersEquityRatio debtToShareholdersEquityRatio { get; set; }
    public class DebtToShareholdersEquityRatio
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 4;
        public double UpperBound { get; set; } = 0.8;
        public string ToolTip { get; set; } = "Desired value: < 0.8. Accounting book: < 0.5";
    }

    /// <summary>
    /// Desired value: as small as possible (comparing to total money that flows in the company)
    /// </summary>
    public LongTermDebt longTermDebt { get; set; }
    public class LongTermDebt
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 2;
        public double DesiredValue { get; set; } = 0;
        public string ToolTip { get; set; } = "Desired value: as small as possible (comparing to total money that flows in the company)";
    }
    
    /// <summary>
    /// No preferred stock is favoured as it is not pre-tax deductible
    /// </summary>
    public PreferredStock preferredStock { get; set; }
    public class  PreferredStock
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 1;
        public double DesiredValue { get; set; } = 0;
        public string ToolTip { get; set; } = "No preferred stock is favoured as it is not pre-tax deductible";
    }

    /// <summary>
    /// Desired value: 30% (< 15% is already bad), accounting book says > 8% is good
    /// </summary>
    public ReturnOnShareHoldersEquityRatio returnOnShareHoldersEquityRatio { get; set; }

    public class ReturnOnShareHoldersEquityRatio
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 4;
        public double DesiredValue { get; set; } = 0.3;
        public double LowerBound { get; set; } = 0.1;
        public string ToolTip { get; set; } = "Desired value: 30% (< 10% is already bad)";
    }
    
    
    /// <summary>
    /// Shows if a company buys back stocks => the smaller, the better
    /// </summary>
    public NetOfIssuanceStock netOfIssuanceStock { get; set; }
    public class NetOfIssuanceStock
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 2;
        public double DesiredValue { get; set; } = Int32.MinValue;
        public string ToolTip { get; set; } = "Shows if a company buys back stocks => the smaller, the better";
    }
    
    /// <summary>
    /// Has to be as close as possible to 50% (0.5)
    /// </summary>
    public InvestingToOperatingCashFlowRatio investingToOperatingCashFlowRatio { get; set; }
    public class InvestingToOperatingCashFlowRatio
    {
        public double Value { get; set; }
        public int Weight { get; set; } = 2;
        public double DesiredValue { get; set; } = 0.5;
        public string ToolTip { get; set; } = "To be as close as possible to 50%";
    }
    
    public Score FinalScore { get; set; }
    public class Score
    {
        public double Value { get; set; }
        public string ToolTip { get; set; } = "Acceptable score > 10. Great score around 15";
    }
}