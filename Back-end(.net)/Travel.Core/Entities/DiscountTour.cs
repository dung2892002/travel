
using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class DiscountTour
{
    public int Id { get; set; }

    public Guid DiscountId { get; set; }

    public Guid TourId { get; set; }

    [JsonIgnore]
    public virtual Discount? Discount { get; set; } = null!;


    [JsonIgnore]
    public virtual Tour? Tour { get; set; } = null!;
}
