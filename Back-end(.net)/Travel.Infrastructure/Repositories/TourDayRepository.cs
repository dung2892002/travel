using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class TourDayRepository(TravelDbContext dbContext) : ITourDayRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task Create(TourDay tourDay)
        {
            await _dbContext.AddAsync(tourDay);
            await _dbContext.SaveChangesAsync();
        }
    }
}
