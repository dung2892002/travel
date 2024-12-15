using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IBookingRoomRepository
    {
        Task Create(BookingRoom bookingRoom);

        Task<PagedResult<BookingRoom>> GetByUser(Guid userId, int? status, int pageNumber);
        Task<IEnumerable<BookingRoom>> GetByHotel(Guid hotelId);
        Task<IEnumerable<BookingRoom>> GetByRoom(Guid roomId);
        Task<IEnumerable<BookingRoom>> GetExpiredBookings(DateTime expirationTime);
        Task<IEnumerable<BookingRoom>> GetRefundBookings();

        Task<BookingRoom?> GetById(Guid? id);

        Task<int> CountTotalBookingByTime(Guid roomId, DateTime start, DateTime end);
        Task<Refund?> GetHotelRefundByBookingRoom(Guid bookingRoomId, int numberDay);
    }
}
