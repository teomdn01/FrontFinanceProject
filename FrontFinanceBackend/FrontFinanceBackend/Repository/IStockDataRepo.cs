using FrontFinanceBackend.Models;

namespace FrontFinanceBackend.Repository
{
    public interface IStockDataRepo
    {
        public int SaveChanges();

        public Task<List<StockData>> GetAll();

        public StockData? GetById(long id);

        public StockData Insert(StockData value);
        public Task<List<StockData>> FindBySymbolAndType(string symbol, string type);
    }
}
