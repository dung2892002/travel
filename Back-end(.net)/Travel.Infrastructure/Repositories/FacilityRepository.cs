using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class FacilityRepository(TravelDbContext dbContext) : IFacilityRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task<IEnumerable<Facility>> GetFacility(int type)
        {
            var facilities = await _dbContext.Facility.Where(f => f.Type == type).ToListAsync();
            return facilities;
        }
    }
}
