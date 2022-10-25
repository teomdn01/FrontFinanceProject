using FrontFinanceBackend.Models;

namespace FrontFinanceBackend.Repository
{
    public interface IStockBarRepo
    {
        public int SaveChanges();

        public Task<List<StockBar>> GetAll();

        public StockBar? GetById(long id);

        public StockBar Insert(StockBar value);
    }
}
