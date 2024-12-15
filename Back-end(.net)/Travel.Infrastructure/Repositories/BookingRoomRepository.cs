using Microsoft.EntityFrameworkCore;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class BookingRoomRepository(TravelDbContext dbContext) : IBookingRoomRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;

        public async Task<int> CountTotalBookingByTime(Guid roomId, DateTime start, DateTime end)
        {
            var result = await _dbContext.BookingRoom
                .Where(b => b.RoomId == roomId && 
                            b.Status != 2 && 
                            b.CheckInDate < end && b.CheckOutDate > start)
                .SumAsync(b => b.Quantity);
            return result;
        }   

        public async Task Create(BookingRoom bookingRoom)
        {
            await _dbContext.BookingRoom.AddAsync(bookingRoom);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookingRoom>> GetByHotel(Guid hotelId)
        {
            var bookings = await _dbContext.BookingRoom.Include(b => b.Room).Where(b => b.Room.HotelId == hotelId).ToListAsync();
            return bookings;
        }

        public async Task<BookingRoom?> GetById(Guid? id)
        {
            var booking = await _dbContext.BookingRoom.SingleOrDefaultAsync(b => b.Id == id);
            return booking;
        }

        public async Task<IEnumerable<BookingRoom>> GetByRoom(Guid roomId)
        {
            var bookings = await _dbContext.BookingRoom.Where(b => b.RoomId == roomId).ToListAsync();

            return bookings;
        }

        public async Task<PagedResult<BookingRoom>> GetByUser(Guid userId, int? status, int pageNumber)
        {
            var query = _dbContext.BookingRoom
                                            .Include(b => b.Room)
                                                .ThenInclude(r => r.Image)
                                            .Include(b => b.Room)
                                                .ThenInclude(r => r.Hotel)
                                            .Include(b => b.Discount)
                                            .Where(b => b.UserId == userId)
                                            .AsQueryable();

            if (status == 0) query = query.Where(b => b.Status == 0);
            if (status == 1) query = query.Where(b => b.Status == 1);
            if (status == 2) query = query.Where(b => b.Status == 2 || b.Status == 3 || b.Status == 4);

            var totalItems = await query.CountAsync();

            var bookings = await query
                                .Skip((pageNumber -1) * 10)
                                .OrderByDescending(b => b.CreatedAt)
                                .Take(10)
                                .ToListAsync();

            return new PagedResult<BookingRoom>
            {
                Items = bookings,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / 10.0)
            };
        }

        public async Task<IEnumerable<BookingRoom>> GetExpiredBookings(DateTime expirationTime)
        {
            var bookings = await _dbContext.BookingRoom.Where(b => b.CreatedAt <= expirationTime && b.Status == 0).ToListAsync();
            return bookings;
        }

        public async Task<IEnumerable<BookingRoom>> GetRefundBookings()
        {
            var bookings = await _dbContext.BookingRoom.Where(b => b.Status == 3).ToListAsync();
            return bookings;
        }

        public async Task<Refund?> GetHotelRefundByBookingRoom(Guid bookingRoomId, int numberDay)
        {
            var refund = await _dbContext.BookingRoom
                        .Where(br => br.Id == bookingRoomId) 
                        .SelectMany(br => br.Room.Hotel.Refund
                            .Where(r => r.State && r.DayBefore <= numberDay) 
                            .OrderByDescending(r => r.DayBefore)) 
                        .FirstOrDefaultAsync();
            return refund;
        }
    }
}
