using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class Facility
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Type { get; set; }

        [JsonIgnore]
        public virtual ICollection<HotelFacility> HotelFacility { get; set; } = new List<HotelFacility>();


        [JsonIgnore]
        public virtual ICollection<RoomFacility> RoomFacility { get; set; } = new List<RoomFacility>();
    }
}
