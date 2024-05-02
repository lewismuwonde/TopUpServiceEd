using System.ComponentModel.DataAnnotations;

namespace BalanceAPI.ViewModel
{
    public class UpdateBalanceRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Top-up amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
