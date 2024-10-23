using Travel.Core.Interfaces.IServices;
using Quartz;

namespace Travel.Core.Quartz
{
    public class BookingTimeoutJob(IBookingRoomService bookingRoomService) : IJob
    {
        private readonly IBookingRoomService _bookingRoomService = bookingRoomService;

        public async Task Execute(IJobExecutionContext context)
        {
            var expiredBookings = await _bookingRoomService.GetExpiredBookings();

            Console.WriteLine("Quartz: Check booking timeout");
            foreach (var booking in expiredBookings)
            {
                booking.CancelReason = "Timeout";
                await _bookingRoomService.CancelBooking(booking.Id ,booking);
            }
        }
    }
}
