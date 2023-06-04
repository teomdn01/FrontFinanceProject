namespace Core.Contracts.Adapters.Freedom;

public interface ITradierTransactionService
{
    Task<string> PlaceOrder(string symbol, string orderType, string secType);
}