using Core.Contracts;
using Core.Contracts.Adapters.Alpaca;
using Core.Contracts.Adapters.Freedom;
using Core.Contracts.Adapters.InteractiveBrokers;
using Core.Contracts.Adapters.Polygon;
using Core.Contracts.Adapters.Tradier;
using Core.Contracts.Adapters.Trading212;
using Core.Models;
using Core.Models.Finances;
using Core.Models.MarketData;
using Microsoft.Extensions.Logging;

namespace Core.Logic.Services;

public class BrokerMarketDataService : IBrokerMarketDataService
{
    private const string BrokerNotSupportedMessage = "Broker not supported.";

    private readonly ILogger<IBrokerMarketDataService> logger;
    //private readonly ICoinbaseMarketDataService coinbaseMarketDataService;
    private readonly IInteractiveBrokersMarketDataService interactiveBrokersMarketDataService;
    private readonly IFreedomMarketDataService freedomMarketDataService;
    private readonly ITradierMarketDataService tradierMarketDataService;
    private readonly IAlpacaMarketDataService alpacaMarketDataService;
    private readonly ITrading212MarketDataService trading212MarketDataService;
    
    private readonly IPolygonFinancesService polygonFinancesService;

    public BrokerMarketDataService(ILogger<BrokerMarketDataService> logger,
       // ICoinbaseMarketDataService coinbaseMarketDataService,
        IFreedomMarketDataService freedomMarketDataService, 
        IInteractiveBrokersMarketDataService interactiveBrokersMarketDataService, 
       ITradierMarketDataService tradierMarketDataService, 
       IAlpacaMarketDataService alpacaMarketDataService, 
       ITrading212MarketDataService trading212MarketDataService, 
       IPolygonFinancesService polygonFinancesService)
    {
        //this.coinbaseMarketDataService = coinbaseMarketDataService;
        this.freedomMarketDataService = freedomMarketDataService;
        this.interactiveBrokersMarketDataService = interactiveBrokersMarketDataService;
        this.tradierMarketDataService = tradierMarketDataService;
        this.alpacaMarketDataService = alpacaMarketDataService;
        this.trading212MarketDataService = trading212MarketDataService;
        
        this.polygonFinancesService = polygonFinancesService;
        this.logger = logger;
    }
    
    public async Task<StockMarketDataResponse> GetMarketData(MarketDataRequest request)
    {
        StockMarketDataResponse response;
        switch (request.Type)
        {
            // case BrokerType.Coinbase:
            //     response = await coinbaseAuthService.GetPositions();
            //     break;
            case BrokerType.InteractiveBrokers:
                response = await interactiveBrokersMarketDataService.GetMarketData(request.Symbols);
                break;
            case BrokerType.Freedom:
                response = await freedomMarketDataService.GetMarketData(request.Symbols);
                break;
            case BrokerType.Tradier:
                response = await tradierMarketDataService.GetMarketData(request.Symbols, request.AuthToken);
                break;
            case BrokerType.Alpaca:
                response = await alpacaMarketDataService.GetMarketData(request.Symbols, request.AuthToken);
                break;
            case BrokerType.Trading212:
                response = await trading212MarketDataService.GetMarketData(request.Symbols, request.AuthToken);
                break;
            default:
                logger.LogError("Market data not supported for the broker {BrokerType}", request.Type);
                throw new NotSupportedException(BrokerNotSupportedMessage);
        }

        response.BrokerType = request.Type.ToString();
        return response;
    }

    public Task<FinancialStatementsAnalysis> AnalyzeFinancialStatements(string symbol)
    {
        return polygonFinancesService.AnalyzeFinancialStatements(symbol);
    }
}