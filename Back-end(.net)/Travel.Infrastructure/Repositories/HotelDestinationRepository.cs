using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class HotelDestinationRepository(TravelDbContext dbContext) : IHotelDestinationRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;

        public async Task AddRange(List<HotelDestination> hotelDestinations)
        {
            await _dbContext.AddRangeAsync(hotelDestinations);
            await _dbContext.SaveChangesAsync();
        }
    }
}
