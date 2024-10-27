using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class BookingRoom
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid RoomId { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public short Quantity { get; set; }

        public string CustomerName { get; set; } = null!;

        public decimal? Price { get; set; }

        public int? Status { get; set; }

        public string? CancelReason { get; set; }

        public DateTime? CreatedAt { get; set; } 

        [JsonIgnore]
        public virtual Room? Room { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; } 
    }
}
