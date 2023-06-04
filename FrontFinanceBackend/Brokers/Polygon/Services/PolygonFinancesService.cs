using System.Net;
using Brokers.Polygon.Configuration;
using Brokers.Polygon.Models;
using Brokers.Polygon.Utils;
using Core.Contracts.Adapters.Polygon;
using Core.Models;
using Core.Models.Finances;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core;
using Org.Front.Core.Contracts.Helpers;
using Org.Front.Core.Contracts.Models.Brokers;

namespace Brokers.Polygon.Services;

public class PolygonFinancesService : IPolygonFinancesService
{
    protected readonly HttpClient HttpClient;
    protected readonly PolygonConfig PolygonConfig;
    protected readonly ILogger Logger;

    private readonly string financesPath = "vX/reference/financials?ticker={0}&apiKey={1}&limit={2}";
    
    public PolygonFinancesService(HttpClient httpClient,
        IOptions<PolygonConfig> configOptions,
        ILogger<PolygonFinancesService> logger)
    {
        HttpClient = httpClient;
        Logger = logger;
        PolygonConfig = configOptions.Value; 
    }

    public async Task<FinancialStatementsAnalysis> AnalyzeFinancialStatements(string symbol)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
            string.Format(financesPath, symbol, PolygonConfig.ApiKey, "70"));

        var response = await Execute<FinancialStatementsResponse>(request, BrokerOperationNames.GetAnalysis);

        var annualFinancials =
            response.Results.Where(r => r.FiscalPeriod.Equals("FY")).ToList();

        var lastAnnualReport = annualFinancials[0];
        
        var currentYearQuarterlyReports =
            response.Results.Where(r =>  !string.IsNullOrWhiteSpace(r.FiscalYear) && Int32.Parse(r.FiscalYear) >= Int32.Parse(lastAnnualReport.FiscalYear) && r.FiscalPeriod.Contains("Q")).ToList();

        FinancialStatementsAnalysis analysisResponse;
        try
        {
                analysisResponse = GetRatiosAnalysis(
                currentYearQuarterlyReports.Count > 0 ? currentYearQuarterlyReports[0] : lastAnnualReport,
                currentYearQuarterlyReports, annualFinancials);

        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            throw BrokerErrorResponseHelper.CreateBrokerException(HttpStatusCode.InternalServerError, 
                $"Not enough data from company {symbol} financial statements to create analysis", 
                $"Not enough data from company {symbol} financial statements to create analysis", BrokerOperationNames.GetAnalysis, "Polygon", Logger);
        }
        
        var ratioValues = new Dictionary<string, RatioValue>
        {
            ["grossProfitMargin"] = new RatioValue {Value = analysisResponse.grossProfitMargin.Value},
            ["interestExpenseToOperatingIncomeRatio"] = new RatioValue { Value = analysisResponse.interestExpenseToOperatingIncomeRatio.Value },
            ["netIncomeToTotalRevenueRatio"] = new RatioValue { Value = analysisResponse.netIncomeToTotalRevenueRatio.Value },
            ["perShareEarningsVolatilityFactor"] = new RatioValue { Value = analysisResponse.perShareEarningsVolatilityFactor.Value},
            ["operationMarginRatio"] = new RatioValue { Value = analysisResponse.operationMarginRatio.Value },
            ["returnOnAssetsRatio"] = new RatioValue { Value = analysisResponse.returnOnAssetsRatio.Value },
            ["currentAssetsLiabilitiesRatio"] = new RatioValue {Value = analysisResponse.currentAssetsLiabilitiesRatio.Value },
            ["debtToShareholdersEquityRatio"] = new RatioValue { Value = analysisResponse.debtToShareholdersEquityRatio.Value },
            ["longTermDebt"] = new RatioValue { Value = analysisResponse.longTermDebt.Value },
            ["preferredStock"] = new RatioValue { Value = analysisResponse.preferredStock.Value },
            ["returnOnShareHoldersEquityRatio"] = new RatioValue { Value = analysisResponse.returnOnShareHoldersEquityRatio.Value },
            ["netOfIssuanceStock"] = new RatioValue { Value = analysisResponse.netOfIssuanceStock.Value },
            ["investingToOperatingCashFlowRatio"] = new RatioValue { Value = analysisResponse.investingToOperatingCashFlowRatio.Value }
        };
        
        var scorer = new InvestmentScorer();
        var score = scorer.CalculateInvestmentScore(ratioValues);
        analysisResponse.FinalScore = new FinancialStatementsAnalysis.Score()
        {
            Value = score
        };
        return analysisResponse;
    }

    private FinancialStatementsAnalysis GetRatiosAnalysis(Result lastReport, List<Result> currentYearQuarterlyReports, List<Result> annualFinancials)
    {
        FinancialStatementsAnalysis result = new FinancialStatementsAnalysis();
        var incomeStatement = lastReport.Financials.IncomeStatement;
        
        //IS
        ApplyRulesOnIncomeStatement(annualFinancials, incomeStatement, result);
        
        //BS
        var balanceSheet = lastReport.Financials.BalanceSheet;
        ApplyRulesOnBalanceSheet(result, incomeStatement, balanceSheet);

        //CFS
        var cashFlowStatement = lastReport.Financials.CashFlowStatement;
        ApplyRulesOnCashFlowStatement(result, cashFlowStatement);
        return result;
    }

    private static void ApplyRulesOnIncomeStatement(List<Result> annualFinancials, IncomeStatement incomeStatement,
        FinancialStatementsAnalysis result)
    {
        double grossProfit = incomeStatement.GrossProfit.Value;
        double grossProfitMargin = grossProfit / incomeStatement.Revenues.Value;

        result.grossProfitMargin.Value = grossProfitMargin;
        
        //page 62 - NetIncomeToTotalRevenue Ratio
        result.netIncomeToTotalRevenueRatio.Value = incomeStatement.NetIncomeLoss.Value / incomeStatement.Revenues.Value;

        //page 64 - Per share earnings to have an ascneding trend over the past 10 years

        
        double volatilityFactor = annualFinancials
            .Skip(1)
            .Take(Math.Min(9, annualFinancials.Count - 1))
            .Where((financials, index) =>
                financials.Financials.IncomeStatement.BasicEarningsPerShare.Value >
                annualFinancials[index].Financials.IncomeStatement.BasicEarningsPerShare.Value)
            .Sum(_ => 0.1);

        volatilityFactor *= annualFinancials.Count < 10 ? annualFinancials.Count / 10d : 1;



        result.perShareEarningsVolatilityFactor.Value = volatilityFactor;

        //accounting book
        //page 163
        result.operationMarginRatio.Value = incomeStatement.OperatingIncomeLoss.Value / incomeStatement.Revenues.Value;

        //b2 page 163
        //b1 page 51 - InterestExpenseToOperatingIncomeRatio
        result.interestExpenseToOperatingIncomeRatio.Value = incomeStatement.OperatingIncomeLoss.Value /
                                                       incomeStatement.InterestExpenseOperating.Value;
        if (result.interestExpenseToOperatingIncomeRatio.Value > 10d)
        {
            result.interestExpenseToOperatingIncomeRatio.Value = 10d;
        }

    }
    
    private static void ApplyRulesOnCashFlowStatement(FinancialStatementsAnalysis result,
        CashFlowStatement cashFlowStatement)
    {
        result.netOfIssuanceStock.Value = cashFlowStatement.NetCashFlowFromFinancingActivities.Value;
        result.investingToOperatingCashFlowRatio.Value = Math.Abs(cashFlowStatement.NetCashFlowFromInvestingActivities.Value) /
                                                         cashFlowStatement.NetCashFlowFromOperatingActivities.Value;
    }
    
    private static void ApplyRulesOnBalanceSheet(FinancialStatementsAnalysis result, IncomeStatement incomeStatement,
        BalanceSheet balanceSheet)
    {
        //page 105
        result.returnOnAssetsRatio.Value = incomeStatement.NetIncomeLoss.Value / balanceSheet.Assets.Value;
        result.longTermDebt.Value = balanceSheet.NoncurrentLiabilities.Value;
        result.currentAssetsLiabilitiesRatio.Value = balanceSheet.CurrentAssets.Value / balanceSheet.CurrentLiabilities.Value;
        //pag 125
        result.debtToShareholdersEquityRatio.Value = balanceSheet.Liabilities.Value / balanceSheet.Equity.Value;
        //pag 130
        result.preferredStock.Value = incomeStatement.PreferredStockDividendsAndOtherAdjustments.Value;
        //pag 141
        result.returnOnShareHoldersEquityRatio.Value = incomeStatement.NetIncomeLoss.Value / balanceSheet.Equity.Value;
    }


    private async Task<T> Execute<T>(HttpRequestMessage request, string operationName)
    {
        var response = await HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var result = content.FromJson<T>(JsonSerializerBehaviour.SnakeCase);
                return result;
            }
            catch (Exception e)
            {
                var errorMessage = "Could not deserialize data from Polygon";
                Logger.LogError(e, errorMessage);
                throw BrokerErrorResponseHelper.CreateBrokerException(response.StatusCode, content, content, operationName, "Polygon", Logger);
            }
        }
        
        throw BrokerErrorResponseHelper.CreateBrokerException(response.StatusCode, content, content, operationName, "Polygon", Logger);
    }
}
