using Alpaca.Markets;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontFinanceBackend.Models
{
    public class StockBar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public DateTime TimeUtc { get; set; }
        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal Volume { get; set; }

        public decimal Vwap { get; set; }

        public ulong TradeCount { get; set; }

        public virtual StockData StockData { get; set; }
        [ForeignKey("Id")]
        public int StockId { get; set; }


        public StockBar() { }

        public StockBar(IBar bar, int foreignKey)
        {
            this.Symbol = bar.Symbol;
            this.TimeUtc = bar.TimeUtc;
            this.Open = bar.Open;  
            this.High = bar.High;
            this.Low = bar.Low;
            this.Close = bar.Close;
            this.Volume = bar.Volume;
            this.Vwap = bar.Vwap;
            this.TradeCount = bar.TradeCount;
            this.StockId = foreignKey;
        }
    }
}
