using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(
            IUnitOfWork unitOfWork,
            IRepository<User> userRepository,
            IRepository<Wallet> walletRepository,
            IRepository<CurrencyWallet> currencyWalletRepository,
            IRepository<Currency> currencyRepository,
            IRepository<ExchangeRate> exchangeRateRepository)
        {
            _unitOfWork = unitOfWork;
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _currencyWalletRepository = currencyWalletRepository;
            _currencyRepository = currencyRepository;
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task Deposit(int userId, string currencyCode, int amount)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                //valido user
                await WalletValidator.ValidateUserAsync(_userRepository, userId);

                //obtengo wallet de user, si no existe, la genero para facilitar las pruebas.
                var wallet = (await _walletRepository.GetByCustomFilterAsync(w => w.UserId == userId)).FirstOrDefault();
                if (wallet == null)
                {
                    wallet = await this.CreateWalletAsync(userId);
                }

                //valido que la currency depositada sea valida
                var currencyId = await WalletValidator.ValidateCurrencyAsync(_currencyRepository, currencyCode);

                //chequeo si el user ya tiene esta currency en su wallet.  
                var currencyWallet = (await _currencyWalletRepository.GetByCustomFilterAsync(cw => cw.WalletId == wallet.WalletId && cw.CurrencyId == currencyId)).FirstOrDefault();

                //Si no tiene esta currency, genero una nueva currency Wallet
                if (currencyWallet == null)
                {
                    currencyWallet = new CurrencyWallet
                    {
                        WalletId = wallet.WalletId,
                        CurrencyId = currencyId,
                        Amount = amount
                    };
                    await _currencyWalletRepository.AddAsync(currencyWallet);
                }
                else
                {
                    //Si  tiene esta currency, sumo el amount depositado
                    currencyWallet.Amount += amount;
                    await _currencyWalletRepository.UpdateAsync(currencyWallet);
                }

                _unitOfWork.Commit();

            }
            catch (Exception ex)
            {
                //realizo rollback en caso de error
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<Wallet> CreateWalletAsync(int userId)
        {
            var newWallet = new Wallet { UserId = userId };
            await _walletRepository.AddAsync(newWallet);
            return newWallet;
        }

        public async Task Withdraw(int userId, string currencyCode, int amount)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                //valido los datos enviados
                await WalletValidator.ValidateUserAsync(_userRepository, userId);
                var walletId = await WalletValidator.ValidateWalletAsync(_walletRepository, userId);
                var currencyId = await WalletValidator.ValidateCurrencyAsync(_currencyRepository, currencyCode);

                //obtengo los datos de esta currency en su wallet
                var currencyWallet = (await _currencyWalletRepository.GetByCustomFilterAsync(cw => cw.WalletId == walletId && cw.CurrencyId == currencyId)).FirstOrDefault();

                //si no tenia esta currency o quiere realizar un withdraw mayor a el monto permitido, arrojo error
                if (currencyWallet == null || currencyWallet.Amount < amount)
                {
                    throw new InvalidOperationException("Insufficient balance");
                }

                //resto el monto retirado y actualizo la DB.
                currencyWallet.Amount -= amount;
                await _currencyWalletRepository.UpdateAsync(currencyWallet);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {               
                //realizo rollback en caso de error
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<Dictionary<string, int>> GetBalance(long userId)
        {

            _unitOfWork.BeginTransaction();

            //valido que el usuario exista y tenga una wallet.
            await WalletValidator.ValidateUserAsync(_userRepository, userId);
                var walletId = await WalletValidator.ValidateWalletAsync(_walletRepository, userId);

                //obtengo todas las currency de la wallet para realizar la suma
                var currencyWallets = await _currencyWalletRepository.GetByCustomFilterAsync(cw => cw.WalletId == walletId);

                //genero un diccionario con todas sus currency y sus amount.
                var balanceDictionary = currencyWallets
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
            _unitOfWork.BeginTransaction();

            try
            {
                //valido que la wallet y las currencies existan
                var walletId = await WalletValidator.ValidateWalletAsync(_walletRepository, userId);
                var sourceCurrencyId = await WalletValidator.ValidateCurrencyAsync(_currencyRepository, sourceCurrencyCode);
                var targetCurrencyId = await WalletValidator.ValidateCurrencyAsync(_currencyRepository, targetCurrencyCode);

                //valido que el usuario tenga la moneda a convertir en su wallet y que tenga suficiente monto
                var sourceCurrencyWallet = (await _currencyWalletRepository.GetByCustomFilterAsync(cw => cw.WalletId == walletId && cw.CurrencyId == sourceCurrencyId)).FirstOrDefault();
              
                if (sourceCurrencyWallet == null || sourceCurrencyWallet.Amount < amount)
                {
                    throw new InvalidOperationException("Insufficient balance.");
                }

                //valido que exista el exchange rate entre las monedas
                var exchangeRate = (await _exchangeRateRepository.GetByCustomFilterAsync(er => er.SourceCurrencyCode == sourceCurrencyCode && er.TargetCurrencyCode == targetCurrencyCode)).FirstOrDefault();

                if (exchangeRate == null)
                {
                    throw new InvalidOperationException($"Exchange rate not found for '{sourceCurrencyCode}' to '{targetCurrencyCode}'.");
                }

                //calculo el monto a convertir y realizo las modificaciones
                var targetAmount = amount * exchangeRate.Rate;

                sourceCurrencyWallet.Amount -= amount;
                await _currencyWalletRepository.UpdateAsync(sourceCurrencyWallet);

                //consulto si el usuario tiene la moneda destino en su wallet, si no la tiene genero el registro. 
                var targetCurrencyWallet = (await _currencyWalletRepository.GetByCustomFilterAsync(cw => cw.WalletId == walletId && cw.CurrencyId == targetCurrencyId)).FirstOrDefault();

                if (targetCurrencyWallet == null)
                {
                    targetCurrencyWallet = new CurrencyWallet
                    {
                        WalletId = walletId,
                        CurrencyId = targetCurrencyId,
                        Amount = (int)targetAmount
                    };
                    await _currencyWalletRepository.AddAsync(targetCurrencyWallet);
                }
                else
                {
                    //Si el usuario contiene la moneda en su wallet, le sumo el amount correspondiente
                    targetCurrencyWallet.Amount += (int)targetAmount;
                    await _currencyWalletRepository.UpdateAsync(targetCurrencyWallet);
                }


                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                //realizo rollback  en caso de error
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
