using Travel.Core.DTOs;

namespace Travel.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UserDTO userDTO);
        List<string> GetUserRoles(string token);

        string GetUserId(string token);
    }
}
