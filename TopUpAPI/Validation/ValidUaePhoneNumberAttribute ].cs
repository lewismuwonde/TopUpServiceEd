using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TopUpAPI.Validation
{
    public class ValidUaePhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phoneNumber = value as string;
            if (phoneNumber == null)
            {
                return new ValidationResult("Phone number must be a string.");
            }

            // UAE phone number regex pattern
            var regexPattern = @"^\+?971[0-9]{9}$";

            if (!Regex.IsMatch(phoneNumber, regexPattern))
            {
                return new ValidationResult("Invalid UAE phone number format.");
            }

            return ValidationResult.Success;
        }
    }
}
