using TopUpAPI.Services.Users;
using TopUpDB.Entity;
using TopUpDB.Interface;

namespace TopUpAPI.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUser _iUser;
        public UserService(IUser iUser)
        {
            _iUser = iUser;
           
        }
        public async Task<User> GetUserById(long userId)
        {
            return await _iUser.GetUserById(userId);
        }
    }
}
