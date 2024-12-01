using System.Threading.Tasks;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IBookingRoomService
    {
        Task<IEnumerable<BookingRoom>> GetByHotel(Guid hotelId);
        Task<IEnumerable<BookingRoom>> GetByRoom(Guid roomId);
        Task<IEnumerable<BookingRoom>> GetByUser(Guid userId);
        Task<IEnumerable<BookingRoom>> GetExpiredBookings();
        Task Create(BookingRoom booking);
        Task<bool> CancelBooking(Guid id, string reason);
    }
}
