namespace Travel.Core.Entities
{
    public class BookingFlight
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int FlightId { get; set; }

        public short TicketClass { get; set; }

        public int NumberSeat { get; set; }

        public string CustomerName { get; set; } = null!;

        public string CustomerIdentityNumber { get; set; } = null!;

        public DateOnly CustomerDob { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; }

        public string? CancelReason { get; set; }

        public virtual Flight Flight { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
