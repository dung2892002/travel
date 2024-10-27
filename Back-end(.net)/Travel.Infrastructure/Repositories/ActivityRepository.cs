using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class ActivityRepository(TravelDbContext dbContext) : IActivityRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task Create(Activity activity)
        {
            await _dbContext.AddAsync(activity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
