using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IRepositories;

namespace Travel.Infrastructure.Data
{
    public class UnitOfWork(TravelDbContext dbContext,
                      IUserRepository userRepository,
                      IRoleRepository roleRepository,
                      IUserRoleRepository userRoleRepository) : IUnitOfWork, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;

        public IUserRepository Users { get; } = userRepository;
        public IRoleRepository Roles { get; } = roleRepository;
        public IUserRoleRepository UserRoles { get; } = userRoleRepository;

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
