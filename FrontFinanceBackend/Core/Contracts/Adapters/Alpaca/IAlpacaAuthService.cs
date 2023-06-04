using Core.Models;
using Core.Models.AccountInfo;

namespace Core.Contracts.Adapters.Alpaca
{
    public interface IAlpacaAuthService
    {
        BrokerAuthPromptResponse GetLinkToken(string redirectLink);
        Task<BrokerAuthResponse> Authenticate(BrokerAuthRequest request);
        string GetAccountId(string accessToken);
        Task<string> GetAccountName(string accessToken);
        
        Task<BrokerPositions> GetPositions(string accessToken);
        Task<BrokerBalance> GetBalance(string accessToken);
    }
}
