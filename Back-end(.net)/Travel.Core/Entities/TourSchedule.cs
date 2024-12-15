using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class TourSchedule
{
    public Guid Id { get; set; }

    public Guid TourId { get; set; }

    public DateTime DateStart { get; set; }

    public decimal Price { get; set; }

    [JsonIgnore]
    public virtual ICollection<BookingTour> BookingTour { get; set; } = new List<BookingTour>();

    public virtual Tour? Tour { get; set; } = null!;
}
