using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class CityRepository(TravelDbContext dbContext) : ICityRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task<IEnumerable<City>> GetAll()
        {
            return await _dbContext.City.Include(c => c.Province).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetByNameContain(string name)
        {
            return await _dbContext.City.Include(c => c.Province).Where(c => c.Name.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetByNameProvince(int provinceId)
        {
            return await _dbContext.City.Include(c => c.Province).Where(c => c.ProvinceId == provinceId).ToListAsync();
        }
    }
}
