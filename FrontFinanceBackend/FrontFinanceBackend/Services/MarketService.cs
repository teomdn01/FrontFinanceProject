using FrontFinanceBackend.Models;
using FrontFinanceBackend.Repository;

namespace FrontFinanceBackend.Services
{
    public class MarketService : IMarketService
    {
        private readonly IStockDataRepo stockDataRepo;
        private readonly IStockBarRepo stockBarRepo;

        public MarketService(IStockDataRepo stockDataRepo, IStockBarRepo stockBarRepo)
        {
            this.stockDataRepo = stockDataRepo;
            this.stockBarRepo = stockBarRepo;
        }

        public async Task<List<StockDataDto>> GetAll()
        {
            List<StockData> data = await stockDataRepo.GetAll();
            List<StockDataDto> result = new List<StockDataDto>();
            data.ForEach(dataItem => {  result.Add(new StockDataDto(dataItem)); });

            return result;
        }
        public StockData? GetById(int guid)
        {
            return stockDataRepo.GetById(guid);
        }

        public void Insert(StockData value)
        {
            stockDataRepo.Insert(value);
            stockDataRepo.SaveChanges();
        }

        public void Insert(StockBar value)
        {
            stockBarRepo.Insert(value);
            stockBarRepo.SaveChanges();
        }

        public async Task<List<StockDataDto>> FindBySymbolAndType(string symbol, string type)
        {
            List<StockData> data = await stockDataRepo.FindBySymbolAndType(symbol, type);
            List<StockDataDto> result = new List<StockDataDto>();
            data.ForEach(dataItem => { result.Add(new StockDataDto(dataItem.Id,dataItem.Timestamp, dataItem.Symbol)); });

            return result;
        }
    }
    
}
