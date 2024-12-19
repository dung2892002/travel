namespace Travel.Core.DTOs
{
    public class StatisticalDay
    {
        public DateTime Day { get; set; }
        public decimal PaymentValue { get; set; }
        public decimal RefundValue { get; set; }
        public int TotalBooking { get; set; }
        public int TotalBookingSuccess { get; set; }
        public int TotalBookingPending { get; set; }
        public int TotalBookingCancel { get; set; }
    }
}
