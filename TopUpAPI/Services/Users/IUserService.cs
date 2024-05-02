using TopUpDB.Entity;
namespace TopUpAPI.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUserById(long userId);
    }
}
