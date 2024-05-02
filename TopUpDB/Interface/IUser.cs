using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUpDB.Entity;

namespace TopUpDB.Interface
{
    public interface IUser
    {
        public Task<User> GetUserByPhonenumber(string phoneNumber);
        public Task<User> GetUserById(long userId);
    }
}
