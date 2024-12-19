using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserById(Guid id);
        Task<User?> GetDetailUser(Guid id);
        Task Add(User user);

        Task<PagedResult<User>> GetUser(string? keyword, int pageSize, int pageNumber);

        Task CreateWallet(Wallet wallet);

        Task<Wallet?> GetWalletByUser(Guid userId);
        Task<PagedResult<WalletDTO>> GetWalletsWithPositiveBalance(int pageNumber);

        Task<Wallet> GetWalletByBookingRoomIdAsync(Guid bookingId);
        Task<Wallet> GetWalletByBookingTourIdAsync(Guid bookingId);
    }
}
