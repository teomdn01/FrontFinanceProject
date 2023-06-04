using Core.Models;
using Core.Models.AccountInfo;

namespace Core.Contracts.Adapters.Freedom;

public interface IFreedomAuthService
{
    Task<BrokerAuthResponse> Authenticate(string username, string password);
    Task<BrokerPositions> GetPositions(string apiKey, string apiSecret);
    Task<BrokerBalance> GetBalance(string apiKey, string apiSecret);
}