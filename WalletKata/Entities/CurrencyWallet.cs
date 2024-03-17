namespace WalletKata.Models
{
    public class CurrencyWallet
    {
        public long CurrencyWalletId { get; set; }
        public long WalletId { get; set; }
        public long CurrencyId { get; set; }
        public int Amount { get; set; }
    }
}
