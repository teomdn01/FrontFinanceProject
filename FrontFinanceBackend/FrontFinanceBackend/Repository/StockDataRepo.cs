using FrontFinanceBackend.Config;
using FrontFinanceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontFinanceBackend.Repository
{
    public class StockDataRepo : IStockDataRepo
    {
        private readonly ApplicationDbContext _dataContext;

        public StockDataRepo(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        public int SaveChanges()
        {
            var result = _dataContext.SaveChanges();

            return result;
        }

        public async Task<List<StockData>> GetAll()
        {
            //return await _dataContext.Set<StockData>().Include(item => item.Bars).ToListAsync();
            return await _dataContext.Set<StockData>().Include(item => item.Bars).ToListAsync();
        }

        public StockData? GetById(long id)
        {
            IQueryable<StockData> query = _dataContext.Set<StockData>().Include(item => item.Bars);
            return query.FirstOrDefault(i => i.Id == id);
        }

        public StockData Insert(StockData value)
        {
            return _dataContext.Set<StockData>().Add(value).Entity;
        }

        public async Task<List<StockData>> FindBySymbolAndType(string symbol, string type)
        {
            return await _dataContext.Set<StockData>().Where(i => i.Symbol.ToLower() == symbol.ToLower() && i.Type.ToLower() == type.ToLower()).ToListAsync();
        }
    }
}
