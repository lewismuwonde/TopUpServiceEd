using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUpDB.Entity;
using TopUpDB.Utility;

namespace TopUpDB.Seeding
{

    public class UserSeeder : IDataSeeder
    { 
        public async Task SeedDataAsync(AppDBContext context)
        {
            if (!context.Users.Any()) 
            {              

                // Hash the password
                (string hashedPassword, byte[] salt) = SecretHasher.HashPassword("@Password123");

                var user1 = new User
                {
                    Name = "John Doe",
                    PhoneNumber = "+971555111222",
                    Email = "johndoe@test.com",
                    Password = hashedPassword,
                    Salt = salt,
                    IsVerified = true,
                    CreateDate = DateTime.Now,                 
                  
                };

                var user2 = new User
                {
                    Name = "Jane Doe",
                    PhoneNumber = "+971555222111",
                    Email = "janedoe@test.com",
                    Password = hashedPassword,
                    Salt = salt,
                    IsVerified = false,
                    CreateDate = DateTime.Now,
                };

                context.Users.AddRange(user1, user2);
                await context.SaveChangesAsync();
            }
        }
    }
}

