using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IPaymentRepository
    {
        Task Create(Payment payment);
        Task<Payment?> GetById(Guid id);
        Task<Payment?> GetByBookingRoom(Guid bookingRoomId);
        Task<Payment?> GetByBookingTour(Guid bookingTourId);
    }
}
