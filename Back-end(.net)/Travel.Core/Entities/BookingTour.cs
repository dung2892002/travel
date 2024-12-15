using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class BookingTour
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid TourScheduleId { get; set; }

    public Guid? DiscountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal Price { get; set; }

    public int Status { get; set; }

    public string? CancelReason { get; set; }

    public string ContactName { get; set; } = null!;

    public string ContactEmail { get; set; } = null!;

    public string ContactPhone { get; set; } = null!;

    public virtual ICollection<BookingTourPeople> BookingTourPeople { get; set; } = new List<BookingTourPeople>();

    public virtual Discount? Discount { get; set; }

    [JsonIgnore]
    public virtual ICollection<Payment> Payment { get; set; } = new List<Payment>();

    public virtual TourSchedule? TourSchedule { get; set; } = null!;

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
