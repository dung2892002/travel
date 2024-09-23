using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class UserRoleRepository(TravelDbContext dbContext) : IUserRoleRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task AddAsync(UserRole userRole)
        {
            await _dbContext.UserRole.AddAsync(userRole);
        }
    }
}
