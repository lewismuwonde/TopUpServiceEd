using TopUpAPI.ViewModel;
using TopUpDB.Entity;

namespace TopUpAPI.Services.Beneficiaries
{
    public interface IBeneficiariesService
    {
        public Task<bool> ValidateActiveBeneficiariesCount(int userId);
        public Task<List<BeneficiaryVM>> GetActiveBeneficiaries(int userId);
        public Task<AddBeneficiaryModel> AddBeneficiary(AddBeneficiaryModel request, int userId);
        public Task<TopUpBeneficiary> GetBeneficiaryById(int beneficiaryId);
    }
}
