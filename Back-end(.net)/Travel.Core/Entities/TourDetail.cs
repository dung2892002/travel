using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class TourDetail
{
    public int Id { get; set; }

    public Guid TourId { get; set; }

    public string Description { get; set; } = null!;

    public short Type { get; set; }

    [JsonIgnore]
    public virtual Tour? Tour { get; set; } = null!;
}
