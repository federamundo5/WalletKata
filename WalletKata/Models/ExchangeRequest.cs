using System.ComponentModel.DataAnnotations;

namespace WalletKata.Models
{

    public class ExchangeRequest
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "SourceCurrencyCode is required")]
        public string SourceCurrencyCode { get; set; }

        [Required(ErrorMessage = "TargetCurrencyCode is required")]
        public string TargetCurrencyCode { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public int Amount { get; set; }
    }

}
