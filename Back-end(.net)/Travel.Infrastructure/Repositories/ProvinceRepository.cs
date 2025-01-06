using Microsoft.EntityFrameworkCore;
using Travel.Core.DTOs;
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

        public async Task<IEnumerable<ProvinceAds>> GetByHotel()
        {
            var province = await _dbContext.Province.Select(
                p => new ProvinceAds
                {
                    Id = p.Id,
                    Name = p.Name,
                    TotalHotel = p.City.SelectMany(c => c.Hotel).Count(),
                })
                .OrderByDescending(p => p.TotalHotel)
                .Take(5)
                .ToListAsync();

            return province;
        }

        public async Task<IEnumerable<Province>> GetByName(string key)
        {
            return await _dbContext.Province.Where(p => p.Name.Contains(key)).ToListAsync();
        }

        public async Task<IEnumerable<ProvinceAds>> GetByTour()
        {
            var province = await _dbContext.Province.Select(
                p => new ProvinceAds
                {
                    Id = p.Id,
                    Name = p.Name,
                    TotalTour = p.City
                     .SelectMany(c => c.TourCity)
                     .Select(tc => tc.TourId)
                     .Distinct()
                     .Count()
                })
                .OrderByDescending(p => p.TotalTour)
                .Take(5)
                .ToListAsync();

            return province;
        }
    }
}
