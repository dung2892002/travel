using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class BookingRoom
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid RoomId { get; set; }
        public Guid? DiscountId { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public short Quantity { get; set; }

        public string ContactName { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;

        public decimal Price { get; set; } = 0;

        public int Status { get; set; } = 0;

        public string? CancelReason { get; set; }

        public DateTime? CreatedAt { get; set; } 

        public virtual Room? Room { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }

        public virtual Discount? Discount { get; set; }

        [JsonIgnore]
        public virtual List<Payment> Payment { get; set; } = new List<Payment>();
    }
}
