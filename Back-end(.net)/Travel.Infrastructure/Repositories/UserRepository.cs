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
    }
}
