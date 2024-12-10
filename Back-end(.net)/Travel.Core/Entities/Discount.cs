using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class Discount
{
    public Guid Id { get; set; }

    public int Percent { get; set; }

    public decimal MinPrice { get; set; }

    public decimal MaxDiscount { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    /// <summary>
    /// 0: Toàn hệ thống
    /// 1: Của riêng đối tác
    /// </summary>
    public int Type { get; set; }

    public Guid UserId { get; set; }

    [JsonIgnore]
    public virtual ICollection<BookingRoom> BookingRoom { get; set; } = new List<BookingRoom>();

    [JsonIgnore]
    public virtual ICollection<BookingTour> BookingTour { get; set; } = new List<BookingTour>();

    public virtual ICollection<DiscountHotel> DiscountHotel { get; set; } = new List<DiscountHotel>();

    public virtual ICollection<DiscountTour> DiscountTour { get; set; } = new List<DiscountTour>();

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
