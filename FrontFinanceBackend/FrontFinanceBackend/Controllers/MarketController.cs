using FrontFinanceBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Alpaca.Markets;
using Brokers.InteractiveBrokers.Models;
using Core.Models.MarketData;
using Environments = Alpaca.Markets.Environments;
using FrontFinanceBackend.Models;
using Microsoft.AspNetCore.Authorization;

namespace FrontFinanceBackend.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMarketService _stockDataService;
        private const string API_KEY = "AKRYX1QT8KS5542V3ZJT";
        private const string SECRET_KEY = "aMBacFhzkZVECXUZs5djugAuYGBVc4hNvywvsF8v";
        private IAlpacaDataClient alpacaDataClient;
        

        public MarketController(IUserService userService, IMarketService stockDataService)
        {
            this._userService = userService;
            this._stockDataService = stockDataService;
            alpacaDataClient = Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, SECRET_KEY));
        }

        private async Task<StockDataDto> GetDataFromApi(string symbol, Timeframe timeframe, BarTimeFrame barTimeFrame, DateTime into, DateTime from)
        {
            var barSet = await alpacaDataClient.ListHistoricalBarsAsync(
            new HistoricalBarsRequest(symbol, from, into, barTimeFrame));

            StockData stockData = new StockData(symbol, timeframe.ToString(), barSet.Items);
            this._stockDataService.Insert(stockData);

            foreach (IBar bar in barSet.Items)
            {
                this._stockDataService.Insert(new StockBar(bar, stockData.Id));
            }

            return new StockDataDto(stockData);
        }

        [HttpGet]
        [Route("{symbol}/{timeframe}")]
        public async Task<StockDataDto> getDataAsync(string symbol, Timeframe timeframe)
        {
            BarTimeFrame barTimeFrame;

            if (timeframe == Timeframe.Daily)
                barTimeFrame = BarTimeFrame.Day;
            else if (timeframe == Timeframe.Hourly)
                barTimeFrame = BarTimeFrame.Hour;
            else
                throw new BadHttpRequestException("Bad timeframe provided. Should be Hourly/Daily");

            var into = DateTime.Now.Subtract(TimeSpan.FromMinutes(20));
            var from = into.Subtract(TimeSpan.FromDays(7));

            long difference = -1;

            List<StockDataDto> stockDatas = await this._stockDataService.FindBySymbolAndType(symbol, timeframe.ToString());
            StockDataDto mostRecentStock = null;

            if (stockDatas != null && stockDatas.Count > 0)
            {
                mostRecentStock = stockDatas[stockDatas.Count - 1];
                long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                difference = now - mostRecentStock.Timestamp;
            }
            
            //we call the Alpaca API if we do not store information about the given symbol or if it is older than 1 hour
            if (difference == -1 || difference > 3600000)
            {
                return await GetDataFromApi(symbol, timeframe, barTimeFrame, into, from);
            }
            else
            {
                StockData data = this._stockDataService.GetById(mostRecentStock.Id);
                return new StockDataDto(data);
            }


        }

        [HttpGet]
        [Route("comparison/{symbol}/{timeframe}")]
        public async Task<List<DataPoint>> getComparisonData(string symbol, Timeframe timeframe)
        {
            BarTimeFrame barTimeFrame;

            if (timeframe == Timeframe.Daily)
                barTimeFrame = BarTimeFrame.Day;
            else if (timeframe == Timeframe.Hourly)
                barTimeFrame = BarTimeFrame.Hour;
            else
                throw new BadHttpRequestException("Bad timeframe provided. Should be Hourly/Daily");

            var into = DateTime.Now.Subtract(TimeSpan.FromMinutes(20));
            var from = into.Subtract(TimeSpan.FromDays(7));

            long difference = -1;

            List<StockDataDto> stockDatas = await this._stockDataService.FindBySymbolAndType(symbol, timeframe.ToString());
            StockDataDto mostRecentStock = null;

            if (stockDatas != null && stockDatas.Count > 0)
            {
                mostRecentStock = stockDatas[stockDatas.Count - 1];
                long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                difference = now - mostRecentStock.Timestamp;
            }

            StockDataDto currentStock = null;
            //we call the Alpaca API if we do not already store information about the given symbol or if it is older than 1 hour
            if (difference == -1 || difference > 3600000)
            {
                currentStock = await GetDataFromApi(symbol, timeframe, barTimeFrame, into, from);
            }
            else
            {
                currentStock = new StockDataDto(this._stockDataService.GetById(mostRecentStock.Id));
            }

            var dataPoints = new List<DataPoint>();
            var initialPerformance = currentStock.Bars[0].Vwap;
            foreach (var item in currentStock.Bars)
            {
                var performance = item.Vwap * 100 / initialPerformance;
                dataPoints.Add(new DataPoint(item.Vwap, item.TimeUtc, performance));
            }

            return dataPoints;

        }


    }
}
