using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class RoomFacility
{
    public int Id { get; set; }

    public Guid RoomId { get; set; }

    public int FacilityId { get; set; }

    public virtual Facility? Facility { get; set; } = null!;

    [JsonIgnore]
    public virtual Room? Room { get; set; } = null!;
}
