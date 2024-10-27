using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class BookingTourRepository(TravelDbContext dbContext) : IBookingTourRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task Create(BookingTour bookingTour)
        {
            await _dbContext.BookingTour.AddAsync(bookingTour);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<BookingTour?> GetById(Guid id)
        {
            return await _dbContext.BookingTour.SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<BookingTour>> GetByTour(Guid tourId)
        {
            return await _dbContext.BookingTour.Where(b => b.TourId == tourId).ToListAsync();
        }

        public async Task<IEnumerable<BookingTour>> GetByUser(Guid userId)
        {
            return await _dbContext.BookingTour.Where(b => b.UserId == userId).ToListAsync();   
        }

        public async Task<IEnumerable<BookingTour>> GetExpiredBookings(DateTime expirationTime)
        {
            var bookings = await _dbContext.BookingTour.Where(b => b.CreatedAt <= expirationTime && b.Status == 0).ToListAsync();
            return bookings;
        }
    }
}
