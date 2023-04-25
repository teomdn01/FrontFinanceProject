using Core.Contracts.Adapters.InteractiveBrokers;
using Core.Models;
using Core.Models.MarketData;
using FrontFinanceBackend.Services;
using Microsoft.AspNetCore.Mvc;
using TwoCaptcha.Exceptions;

namespace FrontFinanceBackend.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IInteractiveBrokersTransactionService _interactiveBrokersTransactionService;

        public TransactionsController(IUserService userService,
            IInteractiveBrokersTransactionService interactiveBrokersTransactionService)
        {
            this._userService = userService;
            _interactiveBrokersTransactionService = interactiveBrokersTransactionService;
        }
        
        [HttpPost]
        [Route("market-data")]
        public async Task<List<StockMarketDataResponse>> GetMarketData([FromBody] MarketDataRequest request)
        {
            switch (request.Type)
            {
                case BrokerType.InteractiveBrokers:
                    return await _interactiveBrokersTransactionService.GetMarketData(request.Symbols);
                default:
                    throw new ApiException("Broker type not supported for fetching market data");
            }
        }
    }
}
