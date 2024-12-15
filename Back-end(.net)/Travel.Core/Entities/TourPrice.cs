using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class TourPrice
{
    public Guid Id { get; set; }

    public Guid TourId { get; set; }

    public int AgeStart { get; set; }

    public int? AgeEnd { get; set; }

    public int Percent { get; set; }

    public bool State { get; set; }

    [JsonIgnore]
    public virtual ICollection<BookingTourPeople> BookingTourPeople { get; set; } = new List<BookingTourPeople>();

    [JsonIgnore]
    public virtual Tour? Tour { get; set; } = null!;
}
