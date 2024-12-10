using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class TourNotice
{
    public int Id { get; set; }

    public Guid TourId { get; set; }

    public string Description { get; set; } = null!;

    [JsonIgnore]
    public virtual Tour? Tour { get; set; } = null!;
}
