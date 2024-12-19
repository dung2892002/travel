using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class UserRepository(TravelDbContext travelDbContext) : IUserRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = travelDbContext;

        public async Task Add(User user)
        {
            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _dbContext.User.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetDetailUser(Guid id)
        {
            return await _dbContext.User
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _dbContext.User
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(x => x.Username == username);
        }

        public async Task<PagedResult<User>> GetUser(string? keyword, int pageSize, int pageNumber)
        {
            var query = _dbContext.User.AsQueryable();

            if (keyword != null) query = query.Where(u => u.Email.Contains(keyword));

            var totalItems = await query.CountAsync();

            var users = await query.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();

            return new PagedResult<User>
            {
                Items = users,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (decimal)pageSize)
            };
        }

        public async Task CreateWallet(Wallet wallet)
        {
            await _dbContext.Wallet.AddAsync(wallet);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Wallet?> GetWalletByUser(Guid userId)
        {
            return await _dbContext.Wallet.SingleOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<PagedResult<WalletDTO>> GetWalletsWithPositiveBalance(int pageNumber)
        {
            var query = _dbContext.Wallet.Where(w => w.Balance > 0).AsQueryable();

            var totalItems = await query.CountAsync();
            var wallets = await query.Skip((pageNumber -1) *20).Take(20).Include(w => w.User).ToListAsync();
            var walletsDto = new List<WalletDTO>();

            foreach (var wallet in wallets)
            {
                walletsDto.Add(new WalletDTO
                {
                    UserId = wallet.UserId,
                    Balance = wallet.Balance,
                    BankName = wallet.BankName,
                    BankNumber = wallet.BankNumber,
                    Username = wallet.User.Username,
                    Email = wallet.User.Email,
                    PhoneNumber = wallet.User.PhoneNumber,
                }); 
            }
            return new PagedResult<WalletDTO>
            {
                Items = walletsDto,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / 10.0)
            };
        }


        public async Task<Wallet> GetWalletByBookingRoomIdAsync(Guid bookingId)
        {
            var wallet = await _dbContext.BookingRoom
                        .Where(b => b.Id == bookingId)
                        .Include(b => b.Room)
                            .ThenInclude(r => r.Hotel)
                                .ThenInclude(h => h.User)
                                    .ThenInclude(u => u.Wallet)
                        .Select(b => b.Room.Hotel.User.Wallet)
                        .FirstAsync();

            return wallet;
        }

        public async Task<Wallet> GetWalletByBookingTourIdAsync(Guid bookingId)
        {
            var wallet = await _dbContext.BookingTour
                        .Where(b => b.Id == bookingId)
                        .Include(b => b.TourSchedule)
                            .ThenInclude(r => r.Tour)
                                .ThenInclude(h => h.User)
                                    .ThenInclude(u => u.Wallet)
                        .Select(b => b.TourSchedule.Tour.User.Wallet)
                        .FirstAsync();

            return wallet;
        }
    }
}
