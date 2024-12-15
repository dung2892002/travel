using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class PaymentRepository(TravelDbContext dbContext) : IPaymentRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task Create(Payment payment)
        {
            await _dbContext.AddAsync(payment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Payment?> GetByBookingRoom(Guid bookingRoomId)
        {
            return await _dbContext.Payment.FirstOrDefaultAsync(p => p.BookingRoomId == bookingRoomId && p.Type);
        }

        public async Task<Payment?> GetByBookingTour(Guid bookingTourId)
        {
            return await _dbContext.Payment.FirstOrDefaultAsync(p => p.BookingTourId == bookingTourId && p.Type);
        }

        public async Task<Payment?> GetById(Guid id)
        {
            return await _dbContext.Payment.Where(p => p.Id == id).FirstOrDefaultAsync();
        }
    }
}
