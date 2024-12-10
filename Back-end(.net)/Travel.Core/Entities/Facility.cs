using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class Facility
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    /// <summary>
    /// 1: tiện nghi phổ biến
    /// 2: Tiện nghi đặc biệt
    /// 3: Tiện nghi phòng phòng
    /// </summary>
    public int Type { get; set; }

    [JsonIgnore]
    public virtual ICollection<HotelFacility> HotelFacility { get; set; } = new List<HotelFacility>();

    [JsonIgnore]
    public virtual ICollection<RoomFacility> RoomFacility { get; set; } = new List<RoomFacility>();
}
