using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class BookingTour
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid TourId { get; set; }
        public Guid? DiscountId { get; set; }

        public DateOnly StartDate { get; set; }

        public string ContactName { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;

        public short NumberToddler { get; set; }

        public short NumberPrimaryChildren { get; set; }

        public short NumberAdultPeople { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; }

        public string? CancelReason { get; set; } = null!;

        [JsonIgnore]
        public virtual Tour? Tour { get; set; } = null!;

        [JsonIgnore]
        public virtual User? User { get; set; } = null!;

        [JsonIgnore]
        public virtual Discount? Discount { get; set; }

        [JsonIgnore]
        public virtual List<Payment> Payment { get; set; } = new List<Payment>();
    }
}
