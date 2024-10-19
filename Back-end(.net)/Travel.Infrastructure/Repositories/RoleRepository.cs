using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class RoleRepository(TravelDbContext dbContext) : IRoleRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task<Role?> GetRoleByRoleValue(int roleValue)
        {
            return await _dbContext.Role.SingleOrDefaultAsync(r => r.RoleValue == roleValue);
        }
    }
}
