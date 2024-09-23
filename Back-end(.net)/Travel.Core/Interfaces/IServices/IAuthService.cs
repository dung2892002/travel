using Travel.Core.DTOs;

namespace Travel.Core.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest loginRequest);
        Task<bool> RegisterAsync(RegisterRequest registerRequest);
        Task<bool> CreateAdminAccount(string jwt, RegisterRequest registerRequest);
    }
}
