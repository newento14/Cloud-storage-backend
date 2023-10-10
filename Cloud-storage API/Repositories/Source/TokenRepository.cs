using Cloud_storage_API.Models;
using Cloud_storage_API.Repositories.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Cloud_storage_API.Repositories.Source
{
    public class TokenRepository : ITokenRepository
    {
        private IConfiguration _configuration;
        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(Users user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSetting:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _configuration["JwtSetting:Issuer"],
                audience: _configuration["JwtSetting:Audience"],
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }

        public int ValidateToken(string token)
        {
            if (token == null) return -1;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSetting:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                return userId;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}
