using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TopUpAPI.Utilities;
using TopUpAPI.ViewModel;
using TopUpDB.Entity;
using TopUpDB.Interface;
using TopUpDB.Utility;

namespace TopUpAPI.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IUser _user;

        public AuthService(IUser user)
        {
            _user = user;
        }
        public async Task<User> AuthenticateUser(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Phonenumber) || string.IsNullOrEmpty(request.Password))
            {
                return null; 
            }
           
            var user = await _user.GetUserByPhonenumber(request.Phonenumber);

            if (user == null)
            {
                return null; 
            }
            var isValid = SecretHasher.ValidatePassword(request.Password, user.Salt, user.Password);
            if (!isValid)
            {
                return null; 
            }

            return user; 
        }

        public string GenerateJwtToken(long userId)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Const.JwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, "topup.api.ae"),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
