using Microsoft.AspNetCore.Mvc;
using Moq;
using WalletKata.Controllers;
using WalletKata.Models;
using WalletKata.Repositories.Interfaces;
using WalletKata.Services;
using WalletKata.Services.Interfaces;

namespace WalletKataTest
{
    public class WalletControllerTests
    {
        [Fact]
        public async Task Deposit_ValidRequest_ReturnsOk()
        {
            // Arrange
            var walletServiceMock = new Mock<IWalletService>();
            walletServiceMock.Setup(service => service.Deposit(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Verifiable();

            var controller = new WalletController(walletServiceMock.Object);
            var request = new DepositRequest { UserId = 1, CurrencyCode = "USD", Amount = 100 };

            // Act
            var result = await controller.Deposit(request);

            // Assert
            Assert.IsType<OkResult>(result);
            walletServiceMock.Verify(); 
        }
    
        [Fact]
        public async Task Exchange_InvalidCurrencyCode_ReturnsNotFound()
        {
            // Arrange
            var walletServiceMock = new Mock<IWalletService>();
            walletServiceMock.Setup(service => service.Exchange(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new ArgumentException("Invalid currency code."));

            var controller = new WalletController(walletServiceMock.Object);
            var request = new ExchangeRequest { UserId = 1, SourceCurrencyCode = "RTERUSD", TargetCurrencyCode = "SDFSAEUR", Amount = 100 };

            // Act
            var result = await controller.Exchange(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Exchange failed: Invalid currency code.", notFoundResult.Value);
        }




    }
}