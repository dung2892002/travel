using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class BookingTourService(IUnitOfWork unitOfWork) : IBookingTourService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> CancelBooking(Guid id, BookingTour booking)
        {
            var bookingExisting = await _unitOfWork.BookingsTour.GetById(id);
            if (bookingExisting == null)
            {
                throw new ArgumentException("booking not exist");
            }

            if (bookingExisting.Status != 0)
            {
                throw new InvalidCastException("Can only cancel unpaid bookings");
            }
            bookingExisting.Status = 2;
            bookingExisting.CancelReason = booking.CancelReason;

            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task Create(BookingTour booking)
        {
            //var tourExisting = await _unitOfWork.Tours.GetById(booking.TourScheduleId);
            //if (tourExisting == null)
            //{
            //    throw new ArgumentException("room not exist");
            //}

            booking.Id = Guid.NewGuid();
            booking.CreatedAt = DateTime.Now;
            booking.Status = 0;

            booking.Price = 0;
            await _unitOfWork.BookingsTour.Create(booking);
        }

        public async Task<IEnumerable<BookingTour>> GetByTour(Guid tourId)
        {
            var tourExisting = await _unitOfWork.Tours.GetById(tourId);
            if (tourExisting == null)
            {
                throw new ArgumentException("room not exist");
            }

            var bookings = await _unitOfWork.BookingsTour.GetByTour(tourId);
            return bookings;
        }

        public async Task<IEnumerable<BookingTour>> GetByUser(Guid userId)
        {
            var user = await _unitOfWork.Users.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentException("user not exist");
            }

            var bookings = await _unitOfWork.BookingsTour.GetByUser(userId);
            return bookings;
        }

        public async Task<IEnumerable<BookingTour>> GetExpiredBookings()
        {
            var currentTime = DateTime.Now;

            var expirationTime = currentTime.AddMinutes(-10);

            return await _unitOfWork.BookingsTour.GetExpiredBookings(expirationTime);
        }
    }
}
