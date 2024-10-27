using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class Image
    {
        public int Id { get; set; }

        public string Path { get; set; } = null!;

        public Guid? HotelId { get; set; }

        public int? DestinationId { get; set; }

        public Guid? RoomId { get; set; }

        public Guid? TourId { get; set; }

        [JsonIgnore]
        public virtual Tour? Tour { get; set; }

        [JsonIgnore]
        public virtual Destination? Destination { get; set; }

        [JsonIgnore]
        public virtual Hotel? Hotel { get; set; }

        [JsonIgnore]
        public virtual Room? Room { get; set; }
    }
}
