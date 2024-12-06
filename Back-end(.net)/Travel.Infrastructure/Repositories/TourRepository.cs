using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class TourRepository(TravelDbContext dbContext) : ITourRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task Create(Tour tour)
        {
            await _dbContext.AddAsync(tour);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tour>> GetAll()
        {
            return await _dbContext.Tour.ToListAsync();
        }

        public async Task<IEnumerable<Tour>> GetByCity(int cityId)
        {
            return await _dbContext.Tour.Where(t => t.TourCity.Any(tc => tc.CityId == cityId)).ToListAsync(); 
        }

        public async Task<IEnumerable<Tour>> GetByDepartureCity(int cityId)
        {
            return await _dbContext.Tour.Where(t => t.DepartureCityId == cityId).ToListAsync();
        }

        public async Task<Tour?> GetById(Guid id)
        {
            return await _dbContext.Tour.SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Tour>> GetByPartner(Guid partnerId)
        {
            return await _dbContext.Tour
                .Include(t => t.DepartureCity)
                    .ThenInclude(dc => dc.Province)
                .Where(t => t.UserId == partnerId)
                .ToListAsync();
        }
    }
}
