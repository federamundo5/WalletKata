using Moq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WalletKata.Models;
using WalletKata.Repositories.Interfaces;
using WalletKata.Services;
using Xunit;

namespace WalletKataTest
{
    public class WalletServiceTests
    {
        [Fact]
        public async Task Deposit_ValidRequest_Successful()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IRepository<User>>();
            var walletRepositoryMock = new Mock<IRepository<Wallet>>();
             var currencyWalletRepositoryMock = new Mock<IRepository<CurrencyWallet>>();
             var currencyRepositoryMock = new Mock<IRepository<Currency>>();
             var exchangeRateRepositoryMock = new Mock<IRepository<ExchangeRate>>();

             // Configuro mock
              userRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(new User { UserId = 1 });
              walletRepositoryMock.Setup(repo => repo.GetByCustomFilterAsync(It.IsAny<Expression<Func<Wallet, bool>>>())).ReturnsAsync(new List<Wallet> { new Wallet { WalletId = 1, UserId = 1 } });
              currencyRepositoryMock.Setup(repo => repo.GetByCustomFilterAsync(It.IsAny<Expression<Func<Currency, bool>>>())).ReturnsAsync(new List<Currency> { new Currency { CurrencyId = 1, Code = "USD" } });


               var walletService = new WalletService(
                unitOfWorkMock.Object,
                userRepositoryMock.Object,
                    walletRepositoryMock.Object,
                    currencyWalletRepositoryMock.Object,
                    currencyRepositoryMock.Object,
                    exchangeRateRepositoryMock.Object);

                // Act
                await walletService.Deposit(1, "USD", 100);
        }


        [Fact]
        public async Task Withdraw_InsufficientBalance_ThrowsException()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IRepository<User>>();
            var walletRepositoryMock = new Mock<IRepository<Wallet>>();
            var currencyWalletRepositoryMock = new Mock<IRepository<CurrencyWallet>>();
            var currencyRepositoryMock = new Mock<IRepository<Currency>>();
            var exchangeRateRepositoryMock = new Mock<IRepository<ExchangeRate>>();

            // configuro mocks
            userRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(new User { UserId = 1 });
            walletRepositoryMock.Setup(repo => repo.GetByCustomFilterAsync(It.IsAny<Expression<Func<Wallet, bool>>>())).ReturnsAsync(new List<Wallet> { new Wallet { WalletId = 1, UserId = 1 } });
            currencyRepositoryMock.Setup(repo => repo.GetByCustomFilterAsync(It.IsAny<Expression<Func<Currency, bool>>>())).ReturnsAsync(new List<Currency> { new Currency { CurrencyId = 1, Code = "USD" } });
            currencyWalletRepositoryMock.Setup(repo => repo.GetByCustomFilterAsync(It.IsAny<Expression<Func<CurrencyWallet, bool>>>())).ReturnsAsync(new List<CurrencyWallet> { new CurrencyWallet { WalletId = 1, CurrencyId = 1, Amount = 50 } }); // Simulate insufficient balance

            var walletService = new WalletService(
                unitOfWorkMock.Object,
                userRepositoryMock.Object,
                walletRepositoryMock.Object,
                currencyWalletRepositoryMock.Object,
                currencyRepositoryMock.Object,
                exchangeRateRepositoryMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await walletService.Withdraw(1, "USD", 12100));
        }





    }
}
