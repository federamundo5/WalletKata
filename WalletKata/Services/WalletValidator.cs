using System;
using System.Linq;
using System.Threading.Tasks;
using WalletKata.Models;
using WalletKata.Repositories.Interfaces;

namespace WalletKata.Services
{
    public static class WalletValidator
    {
        public static async Task ValidateUserAsync(IRepository<User> userRepository, long userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }
        }

        public static async Task<long> ValidateWalletAsync(IRepository<Wallet> walletRepository, long userId)
        {
            var wallet = (await walletRepository.GetByCustomFilterAsync(w => w.UserId == userId)).FirstOrDefault();
            if (wallet == null)
            {
                throw new InvalidOperationException("User has no valid wallet.");
            }

            return wallet.WalletId;
        }

        public static async Task<long> ValidateCurrencyAsync(IRepository<Currency> currencyRepository, string currencyCode)
        {
            var currency = (await currencyRepository.GetByCustomFilterAsync(c => c.Code == currencyCode)).FirstOrDefault();
            if (currency == null)
            {
                throw new ArgumentException("Currency not found.");
            }

            return currency.CurrencyId;
        }

    }
}
