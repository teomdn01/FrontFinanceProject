using Core.Models.Finances;
using Core.Models.MarketData;

namespace Core.Contracts;

public interface IBrokerMarketDataService
{
    Task<StockMarketDataResponse> GetMarketData(MarketDataRequest request);
    
    Task<FinancialStatementsAnalysis> AnalyzeFinancialStatements(string symbol);
}