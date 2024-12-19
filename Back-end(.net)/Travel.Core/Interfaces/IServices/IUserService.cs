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
        Task<bool> LockUser(Guid id);
        Task<bool> UnlockUser(Guid id);

        Task<PagedResult<User>> GetUser(string? keyword, int pageSize, int pageNumber);

        Task CreateWallet(Wallet wallet);
        Task<Wallet?> GetWallet(Guid userId);
        Task<PagedResult<WalletDTO>> GetWalletsWithPositiveBalance(int pageNumber);

        Task<bool> UpdateWallet(Guid userId, Wallet wallet);

        Task<bool> AddBalance(Guid userId, decimal value);

        Task<bool> PaymentWallet(Guid userId);
    }
}
