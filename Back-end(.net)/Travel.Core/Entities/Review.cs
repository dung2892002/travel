using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class Review
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public short Point { get; set; }

        public string Description { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public Guid? HotelId { get; set; }

        public int? DestinationId { get; set; }

        public Guid? TourId { get; set; }

        [JsonIgnore]
        public virtual Destination? Destination { get; set; }

        [JsonIgnore]
        public virtual Hotel? Hotel { get; set; }

        [JsonIgnore]
        public virtual Tour? Tour { get; set; }
        
        public virtual User? User { get; set; }

        public virtual ICollection<Image> Image { get; set; } = new List<Image>();
    }
}
