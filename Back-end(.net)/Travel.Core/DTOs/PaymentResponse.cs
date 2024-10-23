namespace Travel.Core.DTOs
{
    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string? PaymentMethod { get; set; }
        public string? OrderDescription { get; set; }
        public Guid? BookingId { get; set; }
        public string? TransactionId { get; set; }
        public string? Token { get; set; }
        public string? VnPayResponseCode { get; set; }
    }
}
