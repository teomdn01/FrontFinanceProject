using Brokers.Tradier.Configuration;
using Brokers.Tradier.Models;
using Brokers.Tradier.Services;
using Core.Contracts.Adapters.Tradier;
using Core.Models;
using Core.Models.AccountInfo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Position = Core.Models.AccountInfo.Position;

public class TradierAuthService : BaseTradierService, ITradierAuthService
{
    
    public TradierAuthService(HttpClient httpClient, IOptions<TradierConfig> configOptions, ILogger<TradierAuthService> logger) : base(httpClient, configOptions, logger)
    {
    }
    public BrokerAuthResponse Authenticate(string username, string password)
    {
        return new BrokerAuthResponse()
        {
            Status = BrokerAuthStatus.Succeeded,
            DisplayMessage = "Access token resides in Tradier brokerage account settings"
        };
    }

    public async Task<BrokerPositions> GetPositions(BrokerBaseRequest request)
    {
        var accountInfo = await GetUserInfo(request.AuthToken);
        var url = String.Format("accounts/{0}/positions",
            accountInfo.profile.account.account_number);

        var response = await Execute<GetPositionsResponse>(url, HttpMethod.Get, request.AuthToken, "Get positions");
        
        var result = new BrokerPositions()
        {
            Positions = new List<Position>()
        };

        if (response.positions == null)
        {
            result.Status = BrokerRequestStatus.Succeeded;
            return result;
        }
        
        response.positions.position.ForEach(p =>
        {
            result.Positions.Add(new Position()
            {
                Amount = p.quantity,
                CostBasis = p.cost_basis,
                Id = p.id,
                Symbol = p.symbol
            });
        });

        result.Status = BrokerRequestStatus.Succeeded;

        return result;
    }

    public async Task<BrokerBalance> GetBalance(BrokerBaseRequest request)
    {
        var accountInfo = await GetUserInfo(request.AuthToken);
        var url = String.Format("accounts/{0}/balances",
            accountInfo.profile.account.account_number);

        var balances = await Execute<GetBalanceRespose>(url, HttpMethod.Get, request.AuthToken, "Get balance");
        return new BrokerBalance()
        {
            PortfolioStockValue = balances.Balances.TotalEquity,
            Cash = balances.Balances.Cash.CashAvailable,
            Status = BrokerRequestStatus.Succeeded
        };

    }
}