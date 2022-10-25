namespace FrontFinanceBackend.Models
{
    public class StockDataDto
    {
        public int Id { get; set; }
        public long Timestamp { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public virtual List<StockBarDto> Bars { get; set; } = new List<StockBarDto>();
        public decimal Average { get; set; }
        public decimal CurrentPrice { get; set; }

        public StockDataDto(StockData stockData)
        {
            this.Id = stockData.Id;
            this.Timestamp = stockData.Timestamp;
            this.Symbol = stockData.Symbol;
            this.Type = stockData.Type;
            stockData.Bars.ForEach(bar => this.Bars.Add(new StockBarDto(bar)));
            this.Average = stockData.Average;
            this.CurrentPrice = stockData.CurrentPrice;
        }
        public StockDataDto(int id, long timestamp, string symbol)
        {
            this.Timestamp = timestamp;
            this.Symbol = symbol;
            this.Id = id;
        }
    }
}
