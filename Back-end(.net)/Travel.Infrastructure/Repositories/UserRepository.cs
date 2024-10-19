using Microsoft.EntityFrameworkCore;
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

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _dbContext.User.Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                    .SingleOrDefaultAsync(x => x.Username == username);
        }

    }
}
