using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IBookingTourService
    {
        Task<IEnumerable<BookingTour>> GetByTour(Guid tourId);
        Task<IEnumerable<BookingTour>> GetByUser(Guid userId);
        Task<IEnumerable<BookingTour>> GetExpiredBookings();
        Task Create(BookingTour booking);
        Task<bool> CancelBooking(Guid id, BookingTour booking);
    }
}
