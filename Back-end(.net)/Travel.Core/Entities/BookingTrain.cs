namespace Travel.Core.Entities
{
    public class BookingTrain
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int TrainId { get; set; }

        public int TicketClass { get; set; }

        public int NumberSeat { get; set; }

        public string CustomerName { get; set; } = null!;

        public DateOnly CustomerDob { get; set; }

        public string CustomerIdentityNumber { get; set; } = null!;

        public DateOnly CreatedAt { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; }

        public string? CancelReason { get; set; }

        public virtual Train Train { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
