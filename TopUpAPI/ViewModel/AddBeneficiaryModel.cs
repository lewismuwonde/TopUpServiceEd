using System.ComponentModel.DataAnnotations;
using TopUpAPI.Validation;

namespace TopUpAPI.ViewModel
{
    public class AddBeneficiaryModel
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string NickName { get; set; }

        [Required]
        [ValidUaePhoneNumber]
        public string PhoneNumber { get; set; }
    }


}
