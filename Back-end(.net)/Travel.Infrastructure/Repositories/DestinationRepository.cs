using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class DestinationRepository(TravelDbContext travelDbContext) : IDestinationRepository, IRepository
    {
        private readonly TravelDbContext _travelDbContext = travelDbContext;
        public async Task<IEnumerable<Destination>> GetByCity(int cityId)
        {
            return await _travelDbContext.Destination.Include(d => d.Image).Where(d => d.CityId == cityId).ToListAsync();
        }
    }
}
