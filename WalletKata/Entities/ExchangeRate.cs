namespace WalletKata.Models
{
    public class ExchangeRate
    {
        public long ExchangeRateId { get; set; }
        public string SourceCurrencyCode { get; set; }
        public string TargetCurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }



}
