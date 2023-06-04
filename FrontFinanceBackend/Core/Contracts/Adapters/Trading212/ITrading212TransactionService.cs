namespace Core.Contracts.Adapters.Trading212;

public interface ITrading212TransactionService
{
    Task<string> PlaceOrder(string symbol, string orderType, string secType);
}