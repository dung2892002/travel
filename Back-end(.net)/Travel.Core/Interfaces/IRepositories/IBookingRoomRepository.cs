using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IBookingRoomRepository
    {
        Task Create(BookingRoom bookingRoom);

        Task<IEnumerable<BookingRoom>> GetByUser(Guid userId);
        Task<IEnumerable<BookingRoom>> GetByHotel(Guid hotelId);
        Task<IEnumerable<BookingRoom>> GetByRoom(Guid roomId);
        Task<IEnumerable<BookingRoom>> GetExpiredBookings(DateTime expirationTime);

        Task<BookingRoom?> GetById(Guid id);

        Task<int> CountTotalBookingByTime(Guid roomId, DateTime start, DateTime end);
    }
}
