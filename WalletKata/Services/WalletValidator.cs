using WalletKata.Repositories;

namespace WalletKata.Services
{
    public static class WalletValidator
    {
        public static void ValidateUser(KataContext context, long userId)
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                throw new ArgumentException($"User not found.");
            }
        }

        public static long ValidateWallet(KataContext context, long userId)
        {
            var wallet = context.Wallets.FirstOrDefault(w => w.UserId == userId);
            if (wallet == null)
            {
                throw new InvalidOperationException($"User has no valid wallet.");
            }

            return wallet.WalletId;
        }

        public static long ValidateCurrency(KataContext context, string currencyCode)
        {
            var currency = context.Currencies.FirstOrDefault(c => c.Code == currencyCode);
            if (currency == null)
            {
                throw new ArgumentException($"Currency  not found.");
            }

            return currency.CurrencyId;
        }
    }
}
