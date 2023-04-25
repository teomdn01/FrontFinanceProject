using Core.Models;

namespace Brokers.Coinbase;

public interface ICoinbaseAuthService
{
    BrokerAuthPromptResponse GetAuthLink(string redirectLink, bool enableTransfers);
    Task<BrokerAuthResponse> Authenticate(BrokerAuthRequest request);
    string GetAccountId(string accessToken);
    Task<BrokerAuthResponse> RefreshAccessToken(string accessToken);
    Task<string> GetAccountName(string accessToken);
}