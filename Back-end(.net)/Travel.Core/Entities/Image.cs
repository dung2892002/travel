using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class Image
    {
        public int Id { get; set; }

        public string Path { get; set; } = null!;

        public int? HotelId { get; set; }

        public int? DestinationId { get; set; }

        public int? RoomId { get; set; }

        public int? ActivityId { get; set; }

        [JsonIgnore]
        public virtual Activity? Activity { get; set; }

        [JsonIgnore]
        public virtual Destination? Destination { get; set; }

        [JsonIgnore]
        public virtual Hotel? Hotel { get; set; }

        [JsonIgnore]
        public virtual Room? Room { get; set; }
    }
}
