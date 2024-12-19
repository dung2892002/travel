namespace Travel.Core.DTOs
{
    public class StatisticalResponse
    {
        public int TotalBooking { get; set; }
        public int TotalBookingSuccess { get; set; }
        public int TotalBookingPending { get; set; }
        public int TotalBookingCancel { get; set; }
        public decimal TotalPaymentAccessValue { get; set; }
        public decimal TotalPaymentRefundValue { get; set; }
        public virtual IEnumerable<StatisticalDay> StatisticalDay { get; set; } = new List<StatisticalDay>();
    }
}
