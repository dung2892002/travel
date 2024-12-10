
using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class DiscountHotel
{
    public int Id { get; set; }

    public Guid DiscountId { get; set; }

    public Guid HotelId { get; set; }

    [JsonIgnore]
    public virtual Discount? Discount { get; set; } = null!;

    [JsonIgnore]
    public virtual Hotel? Hotel { get; set; } = null!;
}
