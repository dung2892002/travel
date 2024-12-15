using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class Tour
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public short NumberOfDay { get; set; }

    public short NumberOfNight { get; set; }

    public short Rating { get; set; }

    public string Transport { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int DepartureCityId { get; set; }

    public Guid UserId { get; set; }

    public virtual City? DepartureCity { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<DiscountTour> DiscountTour { get; set; } = new List<DiscountTour>();

    [JsonIgnore]
    public virtual ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();

    public virtual ICollection<Image> Image { get; set; } = new List<Image>();

    public virtual ICollection<Refund> Refund { get; set; } = new List<Refund>();

    public virtual ICollection<Review> Review { get; set; } = new List<Review>();

    public virtual ICollection<TourCity> TourCity { get; set; } = new List<TourCity>();

    public virtual ICollection<TourDay> TourDay { get; set; } = new List<TourDay>();

    public virtual ICollection<TourDetail> TourDetail { get; set; } = new List<TourDetail>();

    public virtual ICollection<TourNotice> TourNotice { get; set; } = new List<TourNotice>();

    public virtual ICollection<TourPrice> TourPrice { get; set; } = new List<TourPrice>();

    [JsonIgnore]
    public virtual ICollection<TourSchedule> TourSchedule { get; set; } = new List<TourSchedule>();

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
