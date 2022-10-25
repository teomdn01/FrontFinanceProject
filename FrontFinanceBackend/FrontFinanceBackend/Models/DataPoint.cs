namespace FrontFinanceBackend.Models
{
    public class DataPoint
    {
        public DataPoint(decimal realValue, DateTime timestamp, decimal performance)
        {
            RealValue = realValue;
            Timestamp = timestamp;
            Performance = performance;
        }

        public decimal RealValue { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Performance { get; set; }
    }
}
