using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletKata.Models;
using WalletKata.Services;
using WalletKata.Services.Interfaces;

namespace WalletKata.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {

        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }



        /// <summary>
        /// Deposit money into the user's wallet.
        /// </summary>
        /// <param name="request">Deposit request containing user ID, currency code, and amount.</param>
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _walletService.Deposit(request.UserId, request.CurrencyCode, request.Amount);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Deposit failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawalRequest withdrawalRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _walletService.Withdraw(withdrawalRequest.UserId, withdrawalRequest.CurrencyCode, withdrawalRequest.Amount);
                return Ok("Withdrawal successful.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Withdrawal failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpPost("exchange")]
        public async Task<IActionResult> Exchange(ExchangeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _walletService.Exchange(request.UserId, request.SourceCurrencyCode, request.TargetCurrencyCode, request.Amount);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound($"Exchange failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet("balance/{userId}")]
        public async Task<ActionResult> GetBalance(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var balance = await _walletService.GetBalance(userId);
                var balanceList = balance.Where(c => c.Value != 0)
                                         .Select(currencies => new { CurrencyCode = currencies.Key, Amount = currencies.Value })
                                         .ToList();
                return Ok(balanceList);
            }
            catch (ArgumentException ex)
            {
                return NotFound($"Error retrieving balance: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }


    }
}
