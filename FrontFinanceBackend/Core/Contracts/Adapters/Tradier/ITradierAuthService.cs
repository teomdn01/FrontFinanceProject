using Core.Models;
using Core.Models.AccountInfo;

namespace Core.Contracts.Adapters.Tradier;

public interface ITradierAuthService
{
    BrokerAuthResponse Authenticate(string username, string password);
    Task<BrokerPositions> GetPositions(BrokerBaseRequest request);
    Task<BrokerBalance> GetBalance(BrokerBaseRequest request);
}