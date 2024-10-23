using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IBookingRoomRepository
    {
        Task Create(BookingRoom bookingRoom);

        Task<IEnumerable<BookingRoom>> GetByUser(Guid userId);
        Task<IEnumerable<BookingRoom>> GetByHotel(int hotelId);
        Task<IEnumerable<BookingRoom>> GetByRoom(int roomId);
        Task<IEnumerable<BookingRoom>> GetExpiredBookings(DateTime expirationTime);

        Task<BookingRoom?> GetById(Guid id);

        Task<int> CountTotalBookingByTime(int roomId, DateTime start, DateTime end);
    }
}
