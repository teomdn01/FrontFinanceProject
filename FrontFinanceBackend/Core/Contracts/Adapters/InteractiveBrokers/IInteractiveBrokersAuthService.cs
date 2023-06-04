namespace Core.Contracts.Adapters.InteractiveBrokers;

public interface IInteractiveBrokersAuthService
{
    public string GetAuthFormLink();
    Task<string> GetPositions();
    Task<string> GetBalance();

}