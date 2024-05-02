using System.ComponentModel.DataAnnotations;

namespace TopUpAPI.ViewModel
{
    public class LoginRequest
    {
        [Required]
        public string Phonenumber { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
