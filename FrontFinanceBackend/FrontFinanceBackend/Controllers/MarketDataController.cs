using System.IO.Compression;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using Core.Contracts;
using Core.Contracts.Adapters.InteractiveBrokers;
using Core.Models;
using Core.Models.Finances;
using Core.Models.MarketData;
using FrontFinanceBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using TwoCaptcha.Exceptions;

namespace FrontFinanceBackend.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class MarketDataController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBrokerMarketDataService BrokerMarketDataService;

        public MarketDataController(IUserService userService,
            IInteractiveBrokersMarketDataService interactiveBrokersMarketDataService, 
            IBrokerMarketDataService brokerMarketDataService)
        {
            this._userService = userService;
            BrokerMarketDataService = brokerMarketDataService;
        }
        
        [HttpPost]
        [Route("market-data")]
        public async Task<StockMarketDataResponse> GetMarketData([FromBody] MarketDataRequest request)
        {
            return await BrokerMarketDataService.GetMarketData(request);
        }
        
        [HttpGet]
        [Route("finances")]
        public async Task<FinancialStatementsAnalysis> GetFinancialDataAnalysis([FromQuery(Name="symbol")] string symbol)
        {
            // using(var fileStream = new FileStream(...))
            // using(var cryptoStream = new BufferedStream(fileStream))
            // using(var zipStream = new ZipArchive(cryptoStream))
            // { ... }
            
            return await BrokerMarketDataService.AnalyzeFinancialStatements(symbol);
        }
        
        [HttpPost]
        [Route("mpt")]
        public async Task<string> GetFinancialDataAnalysis()
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://q3vp14cx6j.execute-api.eu-central-1.amazonaws.com/default/mpt-docker");
            request.Content = new StringContent(
                "{\"stocks\": [\"AAPL\", \"TSLA\", \"MSFT\"], \"type\": \"sharpe_ratio\"}", Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
