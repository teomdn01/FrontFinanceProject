namespace FrontFinanceBackend.Models
{
    public class StockBarDto
    {
        public DateTime TimeUtc { get; set; }
        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal Volume { get; set; }

        public decimal Vwap { get; set; }
        public ulong TradeCount { get; set; }
        public StockBarDto(StockBar bar)
        {
            this.TimeUtc = bar.TimeUtc;
            this.Open = bar.Open;
            this.High = bar.High;
            this.Low = bar.Low;
            this.Close = bar.Close;
            this.Volume = bar.Volume;
            this.Vwap = bar.Vwap;
            this.TradeCount = bar.TradeCount;
        }
    }
}
