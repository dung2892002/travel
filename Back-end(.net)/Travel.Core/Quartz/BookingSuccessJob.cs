using AutoMapper.Configuration.Annotations;
using Quartz;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Quartz
{
    public class BookingSuccessJob(IBookingRoomService bookingRoomService, IBookingTourService bookingTourService, IUserService userService) : IJob
    {
        private readonly IBookingRoomService _bookingRoomService = bookingRoomService;
        private readonly IBookingTourService _bookingTourService = bookingTourService;
        private readonly IUserService _userService = userService;

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Quartz: Check booking success");
            var successBookingsRoom = await _bookingRoomService.GetSuccessBookings();
            foreach (var booking in successBookingsRoom)
            {
                if (booking.Discount == null || booking.Discount.Type == 0)
                {
                    await _userService.AddBalance(booking.Room.Hotel.UserId, booking.Price);
                } else
                {
                    await _userService.AddBalance(booking.Room.Hotel.UserId, CanculateDiscountValue(booking.Price, booking.Discount));
                }
                await _bookingRoomService.SuccessBooking(booking.Id);
            }

            var successBookingsTour = await _bookingTourService.GetSuccessBookings();
            foreach (var booking in successBookingsTour)
            {                
                if (booking.Discount == null || booking.Discount.Type == 0)
                {
                    await _userService.AddBalance(booking.TourSchedule.Tour.UserId, booking.Price);
                }
                else
                {
                    await _userService.AddBalance(booking.TourSchedule.Tour.UserId, CanculateDiscountValue(booking.Price, booking.Discount));
                }
                await _bookingTourService.SuccessBooking(booking.Id);
            }
        }

        private static decimal CanculateDiscountValue(decimal price, Discount discount)
        {
            var value = Math.Round(price * discount.Percent / 100, 2);
            if (value <= discount.MaxDiscount) return value;
            return discount.MaxDiscount;
        }
    }
}
