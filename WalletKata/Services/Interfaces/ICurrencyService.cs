using WalletKata.Models;

namespace WalletKata.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetAllCurrencies();
    }
}
