using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Travel.Core.DTOs;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Authorization
{
    public class JwtService(IConfiguration configuration) : IJwtService, IService
    {

        private readonly IConfiguration _configuration = configuration;
        public string GenerateToken(UserDTO userDTO)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, userDTO.Username),
                new("Id", userDTO.Id.ToString()),
            };

            foreach (var role in userDTO.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GetUserId(string token)
        {
            var principal = ValidateToken(token);
            var userId = principal.Claims.Where(c => c.Type == "Id").Select(c => c.Value).FirstOrDefault();

            if (userId == null)
            {
                return "null";
            }
            return userId;
        }

        public List<string> GetUserRoles(string token)
        {
            var principal = ValidateToken(token);
            var roles = principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            return roles ?? new List<string>();
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                throw new SecurityTokenException("Invalid token.");
            }
        }
    }
}
