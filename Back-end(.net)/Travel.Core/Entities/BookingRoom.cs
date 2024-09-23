namespace Travel.Core.Entities
{
    public class BookingRoom
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int RoomId { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public short Quantity { get; set; }

        public string CustomerName { get; set; } = null!;

        public decimal Price { get; set; }

        public int Status { get; set; }

        public string CancelReason { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public virtual Room Room { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
