
using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class Favourite
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public Guid? HotelId { get; set; }

    public Guid? TourId { get; set; }

    public virtual Hotel? Hotel { get; set; }

    public virtual Tour? Tour { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
