using Alpaca.Markets;
using System.ComponentModel.DataAnnotations;

namespace FrontFinanceBackend.Models
{
    public class StockData
    {
        [Key]
        public int Id { get; set; }
        public long Timestamp { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public virtual List<StockBar> ?Bars { get; set; }
        public decimal Average { get; set; }
        public decimal CurrentPrice { get; set; }

        public StockData()
        {

        }

        public StockData(string symbol, string type, IReadOnlyList<IBar> bars)
        {
            this.Symbol = symbol;
            this.Type = type;
            this.Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.Average = bars.Average(item => item.Close);
            this.CurrentPrice = bars.Last().Close;
        }
    }
}
