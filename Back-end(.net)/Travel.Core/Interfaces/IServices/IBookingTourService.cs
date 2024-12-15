using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IBookingTourService
    {
        Task<IEnumerable<BookingTour>> GetByTour(Guid tourId);
        Task<PagedResult<BookingTour>> GetByUser(Guid userId, int? status, int pageNumber);
        Task<IEnumerable<BookingTour>> GetExpiredBookings();
        Task Create(BookingTour booking);
        Task<IEnumerable<BookingTour>> GetRefundBookings();
        Task<bool> CancelBooking(Guid id, string reason);
        Task<bool> Refund(Guid id);
    }
}
