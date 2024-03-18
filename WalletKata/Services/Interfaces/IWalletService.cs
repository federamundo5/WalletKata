namespace WalletKata.Services.Interfaces
{
    public interface IWalletService
    {
        Task Deposit(int userId, string currencyCode, decimal amount);
        Task Withdraw(int userId, string currencyCode, decimal amount);
        Task<Dictionary<string, decimal>> GetBalance(long userId);
        Task Exchange(int userId, string sourceCurrencyCode, string targetCurrencyCode, decimal amount);
    }
}
