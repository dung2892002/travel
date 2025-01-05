using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class Destination
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int CityId { get; set; }
    [JsonIgnore]
    public virtual City? City { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<HotelDestination> HotelDestination { get; set; } = new List<HotelDestination>();
    public virtual ICollection<Image> Image { get; set; } = new List<Image>();
}
