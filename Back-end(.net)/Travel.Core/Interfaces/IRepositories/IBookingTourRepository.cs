using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IBookingTourRepository
    {
        Task Create(BookingTour bookingTour);
        Task<IEnumerable<BookingTour>> GetByUser(Guid userId);
        Task<IEnumerable<BookingTour>> GetByTour(Guid tourId);
        Task<IEnumerable<BookingTour>> GetExpiredBookings(DateTime expirationTime);
        Task<BookingTour?> GetById(Guid id);
    }
}
