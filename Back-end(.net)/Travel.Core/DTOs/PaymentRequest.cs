namespace Travel.Core.DTOs
{
    public class PaymentRequest
    {
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public string ReturnUrl { get; set; } = null!;
    }

}
