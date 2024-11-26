using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IUserService
    {
        Task<LoginResponse?> Login(LoginRequest loginRequest);
        Task<UserDTO?> GetDetailUser(Guid id);
        Task<bool> Register(RegisterRequest registerRequest);
        Task<bool> CreateAdminAccount(RegisterRequest registerRequest);
        Task<bool> CreateHotelAccount(RegisterRequest registerRequest);
        Task<bool> CreateTourAccount(RegisterRequest registerRequest);
        Task<bool> ChangePassword(ChangePasswordRequest changePasswordRequest);
        Task<bool> UpdateInfo(Guid id, UserDTO user);
        Task<bool> ChangeAvatar(Guid id, Stream fileStream, string fileName);
    }
}
