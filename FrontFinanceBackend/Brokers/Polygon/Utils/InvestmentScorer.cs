namespace Brokers.Polygon.Utils;

public class InvestmentScorer
{
    private Dictionary<string, RatioScoreSettings> ratioSettings;

    public InvestmentScorer()
    {
        InitializeRatioSettings();
    }

    public double CalculateInvestmentScore(Dictionary<string, RatioValue> ratios)
    {
        double totalScore = 0;

        foreach (var ratio in ratios)
        {
            if (ratioSettings.ContainsKey(ratio.Key))
            {
                var settings = ratioSettings[ratio.Key];
                double ratioScore = CalculateRatioScore(ratio.Value.Value, settings);
                double weightedScore = ratioScore * settings.Weight;
                totalScore += weightedScore;
            }
        }

        return totalScore;
    }

    private double CalculateRatioScore(double value, RatioScoreSettings settings)
    {
        if (settings.TargetValue.HasValue)
        {
            return LinearScoring(value, settings.TargetValue.Value, settings.DesiredValue);
        }
        else if (settings.LowerBound.HasValue && settings.UpperBound.HasValue)
        {
            return RangeScoring(value, settings.LowerBound.Value, settings.UpperBound.Value);
        }
        else if (settings.LowerBound.HasValue)
        {
            return LowerBoundScoring(value, settings.LowerBound.Value);
        }
        else if (settings.UpperBound.HasValue)
        {
            return UpperBoundScoring(value, settings.UpperBound.Value);
        }
        else if (settings.MinimumGood.HasValue && settings.MinimumBad.HasValue)
        {
            return RangeScoring(value, settings.MinimumBad.Value, settings.MinimumGood.Value);
        }

        return 0; // Default score if no scoring criteria is defined
    }

    private double LinearScoring(double value, double targetValue, double desiredValue)
    {
        if (value <= targetValue)
            return 0;
        if (value >= desiredValue)
            return 1;
        return (value - targetValue) / (desiredValue - targetValue);
    }

    private double RangeScoring(double value, double lowerBound, double upperBound)
    {
        if (value <= lowerBound)
            return 0;
        if (value >= upperBound)
            return 1;
        return (value - lowerBound) / (upperBound - lowerBound);
    }

    private double LowerBoundScoring(double value, double lowerBound)
    {
        if (value <= lowerBound)
            return 0;
        if (lowerBound > 1)
        {
            return (value - lowerBound) / lowerBound;
        }
        
        return (value - lowerBound) / (1 - lowerBound);
    }

    private double UpperBoundScoring(double value, double upperBound)
    {
        if (value >= upperBound)
            return 0;
        return (upperBound - value) / upperBound;
    }

    private void InitializeRatioSettings()
    {
        ratioSettings = new Dictionary<string, RatioScoreSettings>
        {
            ["grossProfitMargin"] = new RatioScoreSettings { Weight = 4, LowerBound = 0.4 },
            ["interestExpenseToOperatingIncomeRatio"] = new RatioScoreSettings { Weight = 3, LowerBound = 5.0, UpperBound = 10.0 },
            ["netIncomeToTotalRevenueRatio"] = new RatioScoreSettings { Weight = 4, MinimumBad = 0.1, MinimumGood = 0.2 },
            ["perShareEarningsVolatilityFactor"] = new RatioScoreSettings { Weight = 2, TargetValue = 0.0 },
            ["operationMarginRatio"] = new RatioScoreSettings { Weight = 4, LowerBound = 0.3 },
            ["returnOnAssetsRatio"] = new RatioScoreSettings { Weight = 3, LowerBound = 0.1 },
            ["currentAssetsLiabilitiesRatio"] = new RatioScoreSettings { Weight = 3, LowerBound = 1.0, DesiredValue = 1.5, UpperBound = 5.0 },
            ["debtToShareholdersEquityRatio"] = new RatioScoreSettings { Weight = 4, UpperBound = 0.8 },
            ["longTermDebt"] = new RatioScoreSettings { Weight = 2, DesiredValue = 0.0 },
            ["preferredStock"] = new RatioScoreSettings { Weight = 1, DesiredValue = 0.0 },
            ["returnOnShareHoldersEquityRatio"] = new RatioScoreSettings { Weight = 4, DesiredValue = 0.3, LowerBound = 0.1 },
            ["netOfIssuanceStock"] = new RatioScoreSettings { Weight = 2, DesiredValue = Int32.MinValue },
            ["investingToOperatingCashFlowRatio"] = new RatioScoreSettings { Weight = 2, DesiredValue = 0.5 }
        };
    }
}

public class RatioScoreSettings
{
    public double Weight { get; set; }
    public double? LowerBound { get; set; }
    public double? UpperBound { get; set; }
    public double? MinimumBad { get; set; }
    public double? MinimumGood { get; set; }
    public double? TargetValue { get; set; }
    public double DesiredValue { get; set; }
}

public class RatioValue
{
    public double Value { get; set; }
}
