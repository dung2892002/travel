using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class BookingRoomRepository(TravelDbContext dbContext) : IBookingRoomRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;

        public async Task<int> CountTotalBookingByTime(int roomId, DateTime start, DateTime end)
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

        public async Task<IEnumerable<BookingRoom>> GetByHotel(int hotelId)
        {
            var bookings = await _dbContext.BookingRoom.Include(b => b.Room).Where(b => b.Room.HotelId == hotelId).ToListAsync();
            return bookings;
        }

        public async Task<BookingRoom?> GetById(Guid id)
        {
            var booking = await _dbContext.BookingRoom.SingleOrDefaultAsync(b => b.Id == id);
            return booking;
        }

        public async Task<IEnumerable<BookingRoom>> GetByRoom(int roomId)
        {
            var bookings = await _dbContext.BookingRoom.Where(b => b.RoomId == roomId).ToListAsync();

            return bookings;
        }

        public async Task<IEnumerable<BookingRoom>> GetByUser(Guid userId)
        {
            var bookings = await _dbContext.BookingRoom.Where(b => b.UserId == userId).ToListAsync();
            return bookings;
        }

        public async Task<IEnumerable<BookingRoom>> GetExpiredBookings(DateTime expirationTime)
        {
            var bookings = await _dbContext.BookingRoom.Where(b => b.CreatedAt <= expirationTime && b.Status == 0).ToListAsync();
            return bookings;
        }
    }
}
