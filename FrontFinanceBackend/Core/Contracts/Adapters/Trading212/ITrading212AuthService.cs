using Core.Models;
using Core.Models.AccountInfo;

namespace Core.Contracts.Adapters.Trading212;

public interface ITrading212AuthService
{
    Task<BrokerAuthResponse> Authenticate(string username, string password);
    Task<BrokerPositions> GetPositions(BrokerBaseRequest request);
    Task<BrokerBalance> GetBalance(BrokerBaseRequest request);
}