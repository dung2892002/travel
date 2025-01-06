using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class FavouriteRepository(TravelDbContext dbContext) : IFavouriteRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;

        public async Task Create(Favourite favourite)
        {
            await _dbContext.Favourite.AddAsync(favourite);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Favourite favourite)
        {
            _dbContext.Favourite.Remove(favourite);
            await Task.CompletedTask;
        }

        public async Task<int> GetQuantityByHotel(Guid hotelId)
        {
            var quantity = await _dbContext.Favourite
                                    .Where(f => f.HotelId == hotelId)
                                    .CountAsync();

            return quantity;
        }

        public async Task<Favourite?> GetById(int id)
        {
            var favourite = await _dbContext.Favourite.SingleOrDefaultAsync(f => f.Id == id);
            return favourite;
        }

        public async Task<int> GetQuantityByTour(Guid tourId)
        {
            var quantity = await _dbContext.Favourite
                                    .Where(f => f.TourId == tourId)
                                    .CountAsync();

            return quantity;
        }

        public async Task<IEnumerable<Favourite>> GetByUser(Guid userId)
        {
            var query = _dbContext.Favourite
                    .Where(f => f.UserId == userId) // Lọc theo userId
                    .Include(f => f.Hotel).ThenInclude(h => h.Image) // Bao gồm thông tin khách sạn và ảnh
                    .Include(f => f.Hotel).ThenInclude(h => h.City).ThenInclude(c => c.Province)
                    .Include(f => f.Hotel).ThenInclude(h => h.HotelFacility).ThenInclude(hf => hf.Facility) // Bao gồm thông tin tiện nghi
                    .Include(f => f.Tour).ThenInclude(t => t.Image); // Bao gồm thông tin tour và ảnh

            return await query.ToListAsync();
        }

        public async Task<Favourite?> GetUserFavouriteHotel(Guid userId, Guid hotelId)
        {
            var favourite = await _dbContext.Favourite.Where(f => f.UserId == userId && f.HotelId == hotelId).FirstOrDefaultAsync();
            return favourite;
        }

        public async Task<Favourite?> GetUserFavouriteTour(Guid userId, Guid tourId)
        {
            var favourite = await _dbContext.Favourite.Where(f => f.UserId == userId && f.TourId == tourId).FirstOrDefaultAsync();
            return favourite;
        }
    }
}
