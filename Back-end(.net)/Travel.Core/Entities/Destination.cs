using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class Destination
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int CityId { get; set; }

        [JsonIgnore]
        public virtual City? City { get; set; }

        [JsonIgnore]
        public virtual ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();

        [JsonIgnore]
        public virtual ICollection<HotelDestination> HotelDestination { get; set; } = new List<HotelDestination>();
                
        public virtual ICollection<Image> Image { get; set; } = new List<Image>();

        [JsonIgnore]
        public virtual ICollection<Review> Review { get; set; } = new List<Review>();

        [JsonIgnore]
        public virtual ICollection<TourDestination> TourDestination { get; set; } = new List<TourDestination>();
    }
}
