using Core.Models;
using Core.Models.AccountInfo;
using Core.Models.Auth;
using Org.Front.Core.Contracts.Models.Brokers;

namespace Core.Contracts;

public interface IBrokerAuthService
{
    Task<BrokerAuthResponse> GetBrokerAuthResponse(BrokerAuthRequest request,
        string deviceId = null);

    Task<BrokerAuthPromptResponse> GetBrokerAuthLink(BrokerType brokerType,
        BrokerGetAuthenticationLinkRequest request,
        string userId);

    string GetExternalBrokerAccountId(BrokerType requestType, string authToken);

    Task<BrokerAuthResponse> RefreshToken(BrokerRefreshTokenRequest request);

    Task<string> GetAccountName(string deviceId,
        BrokerType brokerType,
        string authToken);

    Task<BrokerAccountDetails> GetBrokerAccountDetails(BrokerType requestType,
        string authToken,
        string accountId,
        string innerIdByBroker,
        string deviceId);

    Task<BrokerPositions> GetPositions(BrokerBaseRequest request);
    Task<BrokerBalance> GetBalance(BrokerBaseRequest request);
}