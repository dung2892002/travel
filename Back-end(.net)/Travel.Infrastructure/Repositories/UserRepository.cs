using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class UserRepository(TravelDbContext travelDbContext) : IUserRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = travelDbContext;

        public async Task AddAsync(User user)
        {
            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.User.Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(x => x.Username == username);
        }
    }
}
