using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class BookingTourPeople
{
    public int Id { get; set; }

    public Guid BookingTourId { get; set; }

    public int QuantityPeople { get; set; }

    public Guid TourPriceId { get; set; }


    [JsonIgnore]
    public virtual BookingTour? BookingTour { get; set; } = null!;

    public virtual TourPrice? TourPrice { get; set; } = null!;
}
