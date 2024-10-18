using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkBuddy.Api.Entities;
using WorkBuddy.Api.Interfaces;


namespace WorkBuddy.Api.Services
{
    // A token service is created that i can use so i can authenticate users and sign JWTokens
    // Using this solution instead of using sessions and cookies because its more efficient, and works better for mobile devices
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(AppUser user)
        {
            var tokenKey = config["TokenKey"];

            //Check for null token key and  for min length requirements for tokenKey
            if (tokenKey == null) 
            { 
                throw new Exception("Cannot access tokenKey from appsettings"); 
            }
            
            if(tokenKey.Length < 64)
            {
                throw new Exception("The tokenKey needs to be longer (no pun intended) ");
            }

            
            //Claims
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier,user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwt = tokenHandler.WriteToken(token);

            return jwt;

        }
    }
}
