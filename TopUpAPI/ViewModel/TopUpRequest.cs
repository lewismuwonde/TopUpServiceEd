using System.ComponentModel.DataAnnotations;

namespace TopUpAPI.ViewModel
{
    public class TopUpRequest
    {
        [Required]
        public int BeneficiaryId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Top-up amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
