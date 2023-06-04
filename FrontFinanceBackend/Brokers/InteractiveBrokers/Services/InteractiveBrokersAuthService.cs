using Core.Contracts.Adapters.InteractiveBrokers;

namespace Brokers.InteractiveBrokers.Services;

public class InteractiveBrokersAuthService : IInteractiveBrokersAuthService
{
    private const string LoginUrl = "https://localhost:5000";
    
    public string GetAuthFormLink()
    {
        return LoginUrl;
    }

    public Task<string> GetPositions()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetBalance()
    {
        throw new NotImplementedException();
    }
}