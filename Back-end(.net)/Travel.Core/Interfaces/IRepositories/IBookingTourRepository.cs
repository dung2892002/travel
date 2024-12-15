using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IBookingTourRepository
    {
        Task Create(BookingTour bookingTour);
        Task<PagedResult<BookingTour>> GetByUser(Guid userId, int? status, int pageNumber);
        Task<IEnumerable<BookingTour>> GetByTour(Guid tourId);
        Task<IEnumerable<BookingTour>> GetExpiredBookings(DateTime expirationTime);
        Task<BookingTour?> GetById(Guid? id);
        Task<IEnumerable<BookingTour>> GetRefundBookings();
        Task<Refund?> GetTourRefundByBookingTour(Guid bookingTourId, int numberDay);
    }
}
