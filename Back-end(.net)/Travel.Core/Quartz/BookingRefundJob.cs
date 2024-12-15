using Travel.Core.Interfaces.IServices;
using Quartz;

namespace Travel.Core.Quartz
{
    public class BookingRefundJob(IBookingRoomService bookingRoomService, IBookingTourService bookingTourService) : IJob
    {
        private readonly IBookingRoomService _bookingRoomService = bookingRoomService;
        private readonly IBookingTourService _bookingTourService = bookingTourService;

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Quartz: Check booking refund");
            var refundBookingsRoom = await _bookingRoomService.GetRefundBookings();
            foreach (var booking in refundBookingsRoom)
            {
                await _bookingRoomService.Refund(booking.Id);
            }

            var refundBookingsTour = await _bookingTourService.GetRefundBookings();
            foreach (var booking in refundBookingsTour)
            {
                await _bookingTourService.Refund(booking.Id);
            }
        }
    }
}
