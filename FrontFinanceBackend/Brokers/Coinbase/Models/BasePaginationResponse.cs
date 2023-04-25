namespace Brokers.Coinbase.Models
{
    public class BasePaginationResponse<T>
    {
        public T[] Data { get; set; }
        public PaginationParameters Pagination { get; set; }
    }

    public class PaginationParameters
    {
        public string NextUri { get; set; }
        public string PreviousUri { get; set; }
        public int Limit { get; set; }
        public string Order { get; set; }
        public string EndingBefore { get; set; }
        public string StartingAfter { get; set; }
    }

}
