using FrontFinanceBackend.Config;
using FrontFinanceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontFinanceBackend.Repository
{
    public class StockBarRepo : IStockBarRepo
    {
        private readonly ApplicationDbContext _dataContext;

        public StockBarRepo(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        public int SaveChanges()
        {
            var result = _dataContext.SaveChanges();

            return result;
        }

        public async Task<List<StockBar>> GetAll()
        {
            return await _dataContext.Set<StockBar>().ToListAsync();
        }

        public StockBar? GetById(long id)
        {
            IQueryable<StockBar> query = _dataContext.Set<StockBar>();
            return query.FirstOrDefault(i => i.Id == id);
        }

        public StockBar Insert(StockBar value)
        {
            return _dataContext.Set<StockBar>().Add(value).Entity;
        }
    }
}
