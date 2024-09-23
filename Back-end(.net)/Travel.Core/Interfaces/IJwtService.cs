using System.Security.Claims;
using Travel.Core.DTOs;

namespace Travel.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UserDTO userDTO);
        ClaimsPrincipal ValidateToken(string token);
    }
}
