using FrontFinanceBackend.Models;

namespace FrontFinanceBackend.Services
{
    public interface IStockDataService
    {
        public Task<List<StockDataDto>> GetAll();
        public StockData? GetById(int guid);

        public void Insert(StockData value);
        public void Insert(StockBar value);
        public Task<List<StockDataDto>> FindBySymbolAndType(string symbol, string type);
    }
}
