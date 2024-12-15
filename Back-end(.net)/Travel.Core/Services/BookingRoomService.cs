using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class BookingRoomService(IUnitOfWork unitOfWork) : IBookingRoomService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> CancelBooking(Guid id, string reason)
        {
            var bookingExisting = await _unitOfWork.BookingsRoom.GetById(id);
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
                return await _unitOfWork.CompleteAsync() > 0 ;
            }

            var beforeCheckinDate = (bookingExisting.CheckInDate.Date - DateTime.Now.Date).Days;

            if (beforeCheckinDate <= 0)
            {
                throw new InvalidCastException("Cannot cancel booking");
            }

            var refund = await _unitOfWork.BookingsRoom.GetHotelRefundByBookingRoom(id, beforeCheckinDate);

            if (refund == null)
            {
                bookingExisting.Status = 2;
                bookingExisting.CancelReason = reason;
                return await _unitOfWork.CompleteAsync() > 0;
            }


            var payment = await _unitOfWork.Payments.GetByBookingRoom(id);
            var refundValue = Math.Round(payment.Amount * refund.RefundPercent / 100, 0);

            var refundPayment = new Payment
            {
                Id = Guid.NewGuid(),
                BookingRoomId = id,
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

        public async Task Create(BookingRoom booking)
        {
            if (booking.CheckInDate.Date < DateTime.Now.Date || booking.CheckOutDate.Date <= booking.CheckInDate.Date)
            {
                throw new ArgumentException("Invalid check-in or check-out date");
            }
            var roomExisting = await _unitOfWork.Rooms.GetById(booking.RoomId);
            if (roomExisting == null)
            {
                throw new ArgumentException("room not exist");
            }

            var totalBooking = await _unitOfWork.BookingsRoom.CountTotalBookingByTime(booking.RoomId, booking.CheckInDate, booking.CheckOutDate);

            var totalRoomAvailabel = roomExisting.Quantity - totalBooking;
            if (totalRoomAvailabel < booking.Quantity)
            {
                throw new InvalidOperationException($"Can only booking maximum {totalRoomAvailabel} room");
            }
            booking.Id = Guid.NewGuid();
            booking.CreatedAt = DateTime.Now;
            booking.Status = 0;
            booking.Price = roomExisting.Price * booking.Quantity * (booking.CheckOutDate - booking.CheckInDate).Days;
            var discount = await _unitOfWork.Discounts.GetById(booking.DiscountId);
            if (discount == null || discount.Start > DateTime.Now || discount.End < DateTime.Now) booking.DiscountId = null;
            await _unitOfWork.BookingsRoom.Create(booking);
        }

        public async Task<IEnumerable<BookingRoom>> GetByHotel(Guid hotelId)
        {
            var hotelExisting = await _unitOfWork.Hotels.GetById(hotelId);
            if (hotelExisting == null)
            {
                throw new ArgumentException("hotel not exist");
            }

            var bookings = await _unitOfWork.BookingsRoom.GetByHotel(hotelId);
            return bookings;
        }

        public async Task<IEnumerable<BookingRoom>> GetByRoom(Guid roomId)
        {
            var roomExisting = await _unitOfWork.Rooms.GetById(roomId);
            if (roomExisting == null)
            {
                throw new ArgumentException("room not exist");
            }

            var bookings = await _unitOfWork.BookingsRoom.GetByRoom(roomId);
            return bookings;
        }

        public async Task<PagedResult<BookingRoom>> GetByUser(Guid userId, int? status, int pageNumber)
        {
            var user = await _unitOfWork.Users.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentException("user not exist");
            }

            var bookings = await _unitOfWork.BookingsRoom.GetByUser(userId, status, pageNumber);
            return bookings;
        }

        public async Task<IEnumerable<BookingRoom>> GetExpiredBookings()
        {
            var currentTime = DateTime.Now;

            var expirationTime = currentTime.AddMinutes(-10);

            return await _unitOfWork.BookingsRoom.GetExpiredBookings(expirationTime);
        }

        public async Task<IEnumerable<BookingRoom>> GetRefundBookings()
        {
            return await _unitOfWork.BookingsRoom.GetRefundBookings();
        }

        public async Task<bool> Refund(Guid id)
        {
            var booking = await _unitOfWork.BookingsRoom.GetById(id) ?? throw new ArgumentException("booking not exist");
            booking.Status = 4;

            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
