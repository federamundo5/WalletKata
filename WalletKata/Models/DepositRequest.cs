using System.ComponentModel.DataAnnotations;

namespace WalletKata.Models
{
    public class DepositRequest
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "CurrencyCode is required")]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
    }

}
