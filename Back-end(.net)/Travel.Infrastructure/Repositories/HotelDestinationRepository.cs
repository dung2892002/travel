using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class HotelDestinationRepository(TravelDbContext dbContext) : IHotelDestinationRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;

        public async Task AddRange(IEnumerable<HotelDestination> hotelDestinations)
        {
            await _dbContext.Set<HotelDestination>().AddRangeAsync(hotelDestinations);
        }
    }
}
