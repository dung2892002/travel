using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class HotelFacility
{
    public int Id { get; set; }

    public Guid HotelId { get; set; }

    public int FacilityId { get; set; }

    public virtual Facility? Facility { get; set; } = null!;

    [JsonIgnore]
    public virtual Hotel? Hotel { get; set; } = null!;
}
