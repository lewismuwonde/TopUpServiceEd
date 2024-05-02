using TopUpAPI.ViewModel;
using TopUpDB.Entity;

namespace TopUpAPI.Services.Authentication
{
    public interface IAuthService
    {
        string GenerateJwtToken(long userId);
        Task<User> AuthenticateUser(LoginRequest request);
    }
}
