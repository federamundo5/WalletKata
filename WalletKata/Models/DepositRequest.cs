namespace WalletKata.Models
{
    public class DepositRequest
    {
        public int UserId { get; set; }
        public string CurrencyCode { get; set; }
        public int Amount { get; set; }
    }

}
