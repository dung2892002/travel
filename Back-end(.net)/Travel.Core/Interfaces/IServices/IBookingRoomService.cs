using System.Threading.Tasks;
using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IBookingRoomService
    {
        Task<IEnumerable<BookingRoom>> GetByHotel(Guid hotelId);
        Task<IEnumerable<BookingRoom>> GetByRoom(Guid roomId);
        Task<PagedResult<BookingRoom>> GetByUser(Guid userId, int? status, int pageNumber);
        Task<IEnumerable<BookingRoom>> GetExpiredBookings();
        Task<IEnumerable<BookingRoom>> GetRefundBookings();
        Task Create(BookingRoom booking);
        Task<bool> CancelBooking(Guid id, string reason);
        Task<bool> Refund(Guid id);

        Task<IEnumerable<BookingRoom>> GetSuccessBookings();
        Task<bool> SuccessBooking(Guid id);
    }
}
