﻿using System.ComponentModel.DataAnnotations;

namespace WalletKata.Models
{
    public class WithdrawalRequest
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "CurrencyCode is required")]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public int Amount { get; set; }
    }

}
