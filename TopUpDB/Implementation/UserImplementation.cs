using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUpDB.Entity;
using TopUpDB.Interface;

namespace TopUpDB.Implementation
{
    public class UserImplementation : IUser
    {
        public AppDBContext _context;
        public UserImplementation(AppDBContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(long userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByPhonenumber(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
