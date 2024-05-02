using FluentValidation;

namespace TopUpAPI.ViewModel
{
    public class BeneficiaryVM
    {
        public long Id { get; set; }
        public string NickName { get; set; }
        public string PhoneNumber { get; set; }
    }
   
}
