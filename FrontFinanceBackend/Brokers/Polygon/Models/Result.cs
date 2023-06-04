using System.Text.Json.Serialization; 
using System.Collections.Generic; 
using System; 
namespace Brokers.Polygon.Models{ 

    public class Result
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("start_date")]
        public string StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public string EndDate { get; set; }

        [JsonPropertyName("timeframe")]
        public string Timeframe { get; set; }

        [JsonPropertyName("fiscal_period")]
        public string FiscalPeriod { get; set; }

        [JsonPropertyName("fiscal_year")]
        public string FiscalYear { get; set; }

        [JsonPropertyName("cik")]
        public string Cik { get; set; }

        [JsonPropertyName("sic")]
        public string Sic { get; set; }

        [JsonPropertyName("tickers")]
        public List<string> Tickers { get; set; }

        [JsonPropertyName("company_name")]
        public string CompanyName { get; set; }

        [JsonPropertyName("financials")]
        public Financials Financials { get; set; }

        [JsonPropertyName("filing_date")]
        public string FilingDate { get; set; }

        [JsonPropertyName("acceptance_datetime")]
        public DateTime? AcceptanceDatetime { get; set; }

        [JsonPropertyName("source_filing_url")]
        public string SourceFilingUrl { get; set; }

        [JsonPropertyName("source_filing_file_url")]
        public string SourceFilingFileUrl { get; set; }
    }

}