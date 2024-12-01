using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class DiscountRepository(TravelDbContext dbContext) : IDiscountRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task Create(Discount discount)
        {
            await _dbContext.Discount.AddAsync(discount);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateDiscountHotel(DiscountHotel discountHotel)
        {
            await _dbContext.DiscountHotel.AddAsync(discountHotel);
        }

        public async Task CreateDiscountTour(DiscountTour discountTour)
        {
            await _dbContext.DiscountTour.AddAsync(discountTour);
        }

        public async Task<IEnumerable<Discount>> GetByHotel(Guid hotelId, DateTime date, decimal price)
        {
            var discounts = await _dbContext.Discount
                                            .Where(d => d.Start <= date && d.End >= date && d.MinPrice <= price && (d.Type == 0 || d.DiscountHotel.Any(dh => dh.HotelId == hotelId)))
                                            .ToListAsync();
            return discounts;
        }

        public async Task<Discount?> GetById(Guid? id)
        {
            if (!id.HasValue)
                return null;

            var discount = await _dbContext.Discount.Where(d => d.Id == id).SingleOrDefaultAsync();
            return discount;
        }

        public async Task<IEnumerable<Discount>> GetByTour(Guid tourId, DateTime date, decimal price)
        {
            var discounts = await _dbContext.Discount
                                           .Where(d => d.Start <= date && d.End >= date && d.MinPrice <= price && (d.Type == 0 || d.DiscountTour.Any(dh => dh.TourId == tourId)))
                                           .ToListAsync();
            return discounts;
        }

        public async Task<IEnumerable<Discount>> GetByUser(Guid userId)
        {
            var discounts = await _dbContext.Discount.Where(d => d.UserId == userId).ToListAsync();
            return discounts;
        }
    }
}
