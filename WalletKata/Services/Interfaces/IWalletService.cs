namespace WalletKata.Services.Interfaces
{
    public interface IWalletService
    {
        Task Deposit(int userId, string currencyCode, int amount);
        Task Withdraw(int userId, string currencyCode, int amount);
        Task<Dictionary<string, int>> GetBalance(long userId);
        Task Exchange(int userId, string sourceCurrencyCode, string targetCurrencyCode, int amount);
    }
}
