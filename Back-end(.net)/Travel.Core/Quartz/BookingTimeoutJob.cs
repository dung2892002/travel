using Travel.Core.Interfaces.IServices;
using Quartz;

namespace Travel.Core.Quartz
{
    public class BookingTimeoutJob(IBookingRoomService bookingRoomService, IBookingTourService bookingTourService) : IJob
    {
        private readonly IBookingRoomService _bookingRoomService = bookingRoomService;
        private readonly IBookingTourService _bookingTourService = bookingTourService;

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Quartz: Check booking timeout");
            var expiredBookingsRoom = await _bookingRoomService.GetExpiredBookings();
            foreach (var booking in expiredBookingsRoom)
            {
                var CancelReason = "Hết thời gian thanh toán";
                await _bookingRoomService.CancelBooking(booking.Id, CancelReason);
            }

            var expiredBookingsTour = await _bookingTourService.GetExpiredBookings();
            foreach (var booking in expiredBookingsTour)
            {
                booking.CancelReason = "Hết thời gian thanh toán";
                await _bookingTourService.CancelBooking(booking.Id,booking);
            }
        }
    }
}
