using System.ComponentModel.DataAnnotations;
using WalletKata.Models;
using WalletKata.Repositories;
using WalletKata.Repositories.Interfaces;
using WalletKata.Services.Interfaces;

namespace WalletKata.Services
{
    public class WalletService : IWalletService
    {
        private readonly IRepository<Wallet> _walletRepository;
        private readonly IRepository<CurrencyWallet> _currencyWalletRepository;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<ExchangeRate> _exchangeRateRepository;
        private readonly IRepository<User> _userRepository;

        public WalletService(IRepository<User> userRepository, IRepository<Wallet> walletRepository, IRepository<CurrencyWallet> currencyWalletRepository, IRepository<Currency> currencyRepository, IRepository<ExchangeRate> exchangeRateRepository)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _currencyWalletRepository = currencyWalletRepository;
            _currencyRepository = currencyRepository;
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task Deposit(int userId, string currencyCode, int amount)
        {
            // Validar si el usuario existe
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"User not valid.");
            }

            // Obtener la billetera del usuario
            var wallet = (await _walletRepository.GetAllAsync()).FirstOrDefault(w => w.UserId == userId);
            if (wallet == null)
            {
                // Si la billetera no existe, crear una nueva
                wallet = new Wallet { UserId = userId };
                await _walletRepository.AddAsync(wallet);
            }

            // Obtener la moneda
            var currency = (await _currencyRepository.GetAllAsync()).FirstOrDefault(c => c.Code == currencyCode);
            if (currency == null)
            {
                throw new ArgumentException($"Invalid currency code.");
            }

            // Obtener la billetera de la moneda
            var currencyWallet = (await _currencyWalletRepository.GetAllAsync()).FirstOrDefault(cw => cw.WalletId == wallet.WalletId && cw.CurrencyId == currency.CurrencyId);
            if (currencyWallet == null)
            {
                // Si la billetera de la moneda no existe, crear una nueva
                currencyWallet = new CurrencyWallet
                {
                    WalletId = wallet.WalletId,
                    CurrencyId = currency.CurrencyId,
                    Amount = amount
                };
                await _currencyWalletRepository.AddAsync(currencyWallet);
            }
            else
            {
                currencyWallet.Amount += amount;
                await _currencyWalletRepository.UpdateAsync(currencyWallet);
            }
        }

        public async Task Withdraw(int userId, string currencyCode, int amount)
        {
            // Validar si el usuario existe
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"User not valid.");
            }

            // Obtener la billetera del usuario
            var wallet = (await _walletRepository.GetAllAsync()).FirstOrDefault(w => w.UserId == userId);
            if (wallet == null)
            {
                throw new InvalidOperationException($"User has no valid wallet.");
            }

            // Obtener la moneda
            var currency = (await _currencyRepository.GetAllAsync()).FirstOrDefault(c => c.Code == currencyCode);
            if (currency == null)
            {
                throw new ArgumentException($"Invalid currency code.");
            }

            // Obtener la billetera de la moneda
            var currencyWallet = (await _currencyWalletRepository.GetAllAsync()).FirstOrDefault(cw => cw.WalletId == wallet.WalletId && cw.CurrencyId == currency.CurrencyId);
            if (currencyWallet == null || currencyWallet.Amount < amount)
            {
                throw new InvalidOperationException($"Insufficient balance in '{currencyCode}'.");
            }

            // Actualizar el saldo de la billetera de la moneda
            currencyWallet.Amount -= amount;
            await _currencyWalletRepository.UpdateAsync(currencyWallet);
        }

        public async Task<Dictionary<string, int>> GetBalance(long userId)
        {
            // Validar si el usuario existe
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"User not valid.");
            }

            // Obtener la billetera del usuario
            var wallet = (await _walletRepository.GetAllAsync()).FirstOrDefault(w => w.UserId == userId);
            if (wallet == null)
            {
                throw new InvalidOperationException($"User has no valid wallet.");
            }

            // Obtener los saldos de las distintas monedas en la billetera
            var currencyWallets = await _currencyWalletRepository.GetAllAsync();
            var balanceDictionary = currencyWallets
                .Where(cw => cw.WalletId == wallet.WalletId)
                .Join(
                    await _currencyRepository.GetAllAsync(),
                    cw => cw.CurrencyId,
                    c => c.CurrencyId,
                    (cw, c) => new { CurrencyCode = c.Code, Amount = cw.Amount }
                )
                .ToDictionary(item => item.CurrencyCode, item => item.Amount);

            return balanceDictionary;
        }

        public async Task Exchange(int userId, string sourceCurrencyCode, string targetCurrencyCode, int amount)
        {
            // Validar si el usuario existe
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"User not valid.");
            }

            // Obtener la billetera del usuario
            var wallet = (await _walletRepository.GetAllAsync()).FirstOrDefault(w => w.UserId == userId);
            if (wallet == null)
            {
                throw new InvalidOperationException($"User has no valid wallet.");
            }

            // Obtener las entidades Currency correspondientes a las monedas de origen y destino
            var sourceCurrency = (await _currencyRepository.GetAllAsync()).FirstOrDefault(c => c.Code == sourceCurrencyCode);
            var targetCurrency = (await _currencyRepository.GetAllAsync()).FirstOrDefault(c => c.Code == targetCurrencyCode);
            if (sourceCurrency == null || targetCurrency == null)
            {
                throw new ArgumentException($"Invalid currency code.");
            }

            // Obtener la billetera de la moneda de origen
            var sourceCurrencyWallet = (await _currencyWalletRepository.GetAllAsync()).FirstOrDefault(cw => cw.WalletId == wallet.WalletId && cw.CurrencyId == sourceCurrency.CurrencyId);
            if (sourceCurrencyWallet == null || sourceCurrencyWallet.Amount < amount)
            {
                throw new InvalidOperationException($"Insufficient balance in '{sourceCurrencyCode}'.");
            }

            // Obtener el tipo de cambio entre las monedas de origen y destino
            var exchangeRate = (await _exchangeRateRepository.GetAllAsync()).FirstOrDefault(er => er.SourceCurrencyCode == sourceCurrencyCode && er.TargetCurrencyCode == targetCurrencyCode);
            if (exchangeRate == null)
            {
                throw new InvalidOperationException($"Exchange rate not found for converting from '{sourceCurrencyCode}' to '{targetCurrencyCode}'.");
            }

            // Calcular el monto en la moneda de destino
            var targetAmount = amount * exchangeRate.Rate;

            // Actualizar los saldos de las billeteras de las monedas de origen y destino
            sourceCurrencyWallet.Amount -= amount;
            await _currencyWalletRepository.UpdateAsync(sourceCurrencyWallet);

            var targetCurrencyWallet = (await _currencyWalletRepository.GetAllAsync()).FirstOrDefault(cw => cw.WalletId == wallet.WalletId && cw.CurrencyId == targetCurrency.CurrencyId);
            if (targetCurrencyWallet == null)
            {
                targetCurrencyWallet = new CurrencyWallet
                {
                    WalletId = sourceCurrencyWallet.WalletId,
                    CurrencyId = targetCurrency.CurrencyId,
                    Amount = (int)targetAmount
                };
                await _currencyWalletRepository.AddAsync(targetCurrencyWallet);
            }
            else
            {
                targetCurrencyWallet.Amount += (int)targetAmount;
                await _currencyWalletRepository.UpdateAsync(targetCurrencyWallet);
            }
        }




    }
}
