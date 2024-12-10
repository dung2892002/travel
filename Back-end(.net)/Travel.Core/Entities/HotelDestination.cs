using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class HotelDestination
{
    public int Id { get; set; }

    public Guid HotelId { get; set; }

    public int DestinationId { get; set; }

    public virtual Destination? Destination { get; set; } = null!;

    [JsonIgnore]
    public virtual Hotel? Hotel { get; set; } = null!;
}
