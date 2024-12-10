using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class Refund
{
    public int Id { get; set; }

    public Guid? TourId { get; set; }

    public Guid? HotelId { get; set; }

    public int DayBefore { get; set; }

    public int RefundPercent { get; set; }

    [JsonIgnore]
    public virtual Hotel? Hotel { get; set; }

    [JsonIgnore]
    public virtual Tour? Tour { get; set; }
}
