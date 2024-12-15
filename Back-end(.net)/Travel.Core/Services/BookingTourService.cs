using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class BookingTourService(IUnitOfWork unitOfWork) : IBookingTourService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> CancelBooking(Guid id, string reason)
        {
            var bookingExisting = await _unitOfWork.BookingsTour.GetById(id);
            if (bookingExisting == null)
            {
                throw new ArgumentException("booking not exist");
            }

            if (bookingExisting.Status == 2 || bookingExisting.Status == 3)
            {
                throw new InvalidCastException("Booking has  been canceled");
            }

            if (bookingExisting.Status == 0)
            {
                bookingExisting.Status = 2;
                bookingExisting.CancelReason = reason;
                return await _unitOfWork.CompleteAsync() > 0;
            }

            var beforeCheckinDate = (bookingExisting.TourSchedule.DateStart - DateTime.Now.Date).Days;

            if (beforeCheckinDate <= 0)
            {
                throw new InvalidCastException("Cannot cancel booking");
            }

            var refund = await _unitOfWork.BookingsTour.GetTourRefundByBookingTour(id, beforeCheckinDate);

            if (refund == null)
            {
                bookingExisting.Status = 2;
                bookingExisting.CancelReason = reason;
                return await _unitOfWork.CompleteAsync() > 0;
            }


            var payment = await _unitOfWork.Payments.GetByBookingTour(id);
            var refundValue = Math.Round(payment.Amount * refund.RefundPercent / 100, 0);

            var refundPayment = new Payment
            {
                Id = Guid.NewGuid(),
                BookingTourId = id,
                Amount = refundValue,
                CreatedAt = DateTime.Now,
                Type = false,
                TransactionId = 0,
            };

            await _unitOfWork.Payments.Create(refundPayment);
            

            bookingExisting.Status = 3;
            bookingExisting.CancelReason = reason;
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task Create(BookingTour booking)
        {
            var scheduleExisting = await _unitOfWork.Tours.GetTourScheduleById(booking.TourScheduleId) ?? throw new ArgumentException("schedule not exist");

            booking.Id = Guid.NewGuid();
            booking.CreatedAt = DateTime.Now;
            booking.Status = 0;

            decimal price = 0;

            foreach (var bkPeople in booking.BookingTourPeople)
            {
                var tourPrice = await _unitOfWork.Tours.GetTourPriceById(bkPeople.TourPriceId) ?? throw new ArgumentException("tour price not exist");

                price += Math.Round(tourPrice.Percent * scheduleExisting.Price / 100) * bkPeople.QuantityPeople;
            }

            booking.Price = price;

            var discount = await _unitOfWork.Discounts.GetById(booking.DiscountId);
            if (discount == null || discount.Start > DateTime.Now || discount.End < DateTime.Now) booking.DiscountId = null;

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

        public async Task<PagedResult<BookingTour>> GetByUser(Guid userId, int? status, int pageNumber)
        {
            var user = await _unitOfWork.Users.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentException("user not exist");
            }

            var bookings = await _unitOfWork.BookingsTour.GetByUser(userId, status, pageNumber);
            return bookings;
        }

        public async Task<IEnumerable<BookingTour>> GetExpiredBookings()
        {
            var currentTime = DateTime.Now;

            var expirationTime = currentTime.AddMinutes(-10);

            return await _unitOfWork.BookingsTour.GetExpiredBookings(expirationTime);
        }

        public async Task<IEnumerable<BookingTour>> GetRefundBookings()
        {
            return await _unitOfWork.BookingsTour.GetRefundBookings();
        }

        public async Task<bool> Refund(Guid id)
        {
            var booking = await _unitOfWork.BookingsTour.GetById(id) ?? throw new ArgumentException("booking not exist");
            booking.Status = 4;

            return await _unitOfWork.CompleteAsync() > 0;
        }

    }
}
