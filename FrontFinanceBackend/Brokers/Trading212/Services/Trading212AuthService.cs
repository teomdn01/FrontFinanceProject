using System.Net.Mime;
using System.Text;
using Brokers.Trading212.Configuration;
using Brokers.Trading212.Models;
using Brokers.Trading212.Services;
using Core.Contracts.Adapters.Trading212;
using Core.Models;
using Core.Models.AccountInfo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core;
using Org.Front.Core.Contracts.Models.Brokers;
using Position = Core.Models.AccountInfo.Position;

public class Trading212AuthService : BaseTrading212Service, ITrading212AuthService
{
    
    public Trading212AuthService(HttpClient httpClient, IOptions<Trading212Config> configOptions, ILogger<Trading212AuthService> logger) : base(httpClient, configOptions, logger)
    {
    }
    public async Task<BrokerAuthResponse> Authenticate(string username, string password)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "rest/v3/login?skipVersionCheck=false");
        request.Headers.TryAddWithoutValidation("demo", "b17c8abc36b08a228a631f71b97ad46e");
        
        var body = new AuthenticatePayload()
        {
            Username = username,
            Password = password,
            RememberMe = true

        };
        
        request.Content = new StringContent(body.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await Execute<AuthResponse>(request, BrokerOperationNames.Authenticate);
        
        return new BrokerAuthResponse()
        {
            Status = BrokerAuthStatus.Succeeded,
            AccessToken = response.AccountSession,
            Account = new BrokerAccount()
            {
                AccountId = response.AccountId.ToString()
            }
        };
    }

    public async Task<BrokerBalance> GetBalance(BrokerBaseRequest baseRequest)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "rest/trading/v1/accounts/summary");
        request.Headers.TryAddWithoutValidation("Cookie", $"TRADING212_SESSION_LIVE={baseRequest.AuthToken}");
        request.Content = new StringContent("[]", Encoding.UTF8, "application/json");

        var balances = await Execute<AccountSummaryResponse>(request, BrokerOperationNames.GetBalance);
        return new BrokerBalance()
        {
            PortfolioStockValue = balances.Cash.Total,
            Cash = balances.Cash.CashLiquid,
            Status = BrokerRequestStatus.Succeeded
        };
        
    }

    public async Task<BrokerPositions> GetPositions(BrokerBaseRequest baseRequest)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "rest/trading/v1/accounts/summary");
        request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
        request.Headers.TryAddWithoutValidation("Cookie", $"TRADING212_SESSION_LIVE={baseRequest.AuthToken}");
        request.Content = new StringContent("[]", Encoding.UTF8, "application/json");

        var positions = await Execute<AccountSummaryResponse>(request, "Get Positons");
        
        var result = new BrokerPositions()
        {
            Positions = new List<Position>()
        };

        // positions.positions.position.ForEach(p =>
        // {
        //     result.Positions.Add(new Position()
        //     {
        //         Amount = p.quantity,
        //         CostBasis = p.cost_basis,
        //         Id = p.id,
        //         Symbol = p.symbol
        //     });
        // });
        
        result.Status = BrokerRequestStatus.Succeeded;
        
        return result;
    }
}