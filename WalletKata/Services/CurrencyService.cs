using WalletKata.Models;
using WalletKata.Repositories.Interfaces;
using WalletKata.Services.Interfaces;

namespace WalletKata.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyService(IRepository<Currency> currencyRepository, IUnitOfWork unitOfWork)
        {
            _currencyRepository = currencyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencies()
        {
            try
            {
                return await _currencyRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving currencies.", ex);
            }
        }
    }
}
