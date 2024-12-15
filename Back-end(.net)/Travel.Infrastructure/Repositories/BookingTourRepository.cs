using Microsoft.EntityFrameworkCore;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
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

        public async Task<BookingTour?> GetById(Guid? id)
        {
            return await _dbContext.BookingTour
                                    .Include(b => b.TourSchedule)
                                    .SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<BookingTour>> GetByTour(Guid tourId)
        {
            return await _dbContext.BookingTour.ToListAsync();
        }

        public async Task<PagedResult<BookingTour>> GetByUser(Guid userId, int? status, int pageNumber)
        {
            var query = _dbContext.BookingTour
                                            .Include(b => b.TourSchedule)
                                                .ThenInclude(r => r.Tour)
                                                    .ThenInclude(t => t.Image)
                                            .Include(b => b.Discount)
                                            .Where(b => b.UserId == userId)
                                            .AsQueryable();

            if (status == 0) query = query.Where(b => b.Status == 0);
            if (status == 1) query = query.Where(b => b.Status == 1);
            if (status == 2) query = query.Where(b => b.Status == 2 || b.Status == 3 || b.Status == 4);
            var totalItems = await query.CountAsync();

            var bookings = await query
                                .Skip((pageNumber - 1) * 10)
                                .OrderByDescending(b => b.CreatedAt)
                                .Take(10)
                                .ToListAsync();

            return new PagedResult<BookingTour>
            {
                Items = bookings,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / 10.0)
            };
        }

        public async Task<IEnumerable<BookingTour>> GetExpiredBookings(DateTime expirationTime)
        {
            var bookings = await _dbContext.BookingTour.Where(b => b.CreatedAt <= expirationTime && b.Status == 0).ToListAsync();
            return bookings;
        }

        public async Task<IEnumerable<BookingTour>> GetRefundBookings()
        {
            var bookings = await _dbContext.BookingTour.Where(b => b.Status == 3).ToListAsync();
            return bookings;
        }

        public async Task<Refund?> GetTourRefundByBookingTour(Guid bookingTourId, int numberDay)
        {
            var refund = await _dbContext.BookingTour
                         .Where(br => br.Id == bookingTourId)
                         .SelectMany(br => br.TourSchedule.Tour.Refund
                             .Where(r => r.State && r.DayBefore <= numberDay)
                             .OrderByDescending(r => r.DayBefore))
                         .FirstOrDefaultAsync();
            return refund;
        }
    }
}
