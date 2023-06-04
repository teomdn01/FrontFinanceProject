namespace Core.Contracts.Adapters.Freedom;

public interface IFreedomTransactionService
{
    Task<string> PlaceOrder(string symbol, string orderType, string secType);
}