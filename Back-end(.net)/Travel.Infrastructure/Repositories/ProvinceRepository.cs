using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class ProvinceRepository(TravelDbContext dbContext) : IProvinceRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task<IEnumerable<Province>> GetAll()
        {
            return await _dbContext.Province.ToListAsync();
        }

        public async Task<IEnumerable<Province>> GetByName(string key)
        {
            return await _dbContext.Province.Where(p => p.Name.Contains(key)).ToListAsync();
        }
    }
}
