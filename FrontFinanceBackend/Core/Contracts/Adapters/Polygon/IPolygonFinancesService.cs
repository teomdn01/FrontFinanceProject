using Core.Models.Finances;

namespace Core.Contracts.Adapters.Polygon;

public interface IPolygonFinancesService
{
    Task<FinancialStatementsAnalysis> AnalyzeFinancialStatements(string symbol);
}