using TopUpAPI.ViewModel;

namespace TopUpAPI.Services.TopUp
{
    public interface ITopUpService
    {
        List<int> GetTopUpOptions();
        Task<bool> TopUpBeneficiary(TopUpRequest request, int userId); 
    }
}
