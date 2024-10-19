using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int ProvinceId { get; set; }

        public string Image { get; set; } = null!;

        public virtual Province Province { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Airport> Airport { get; set; } = new List<Airport>();

        [JsonIgnore]
        public virtual ICollection<Destination> Destination { get; set; } = new List<Destination>();

        [JsonIgnore]
        public virtual ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();

        [JsonIgnore]
        public virtual ICollection<Tour> Tour { get; set; } = new List<Tour>();

        [JsonIgnore]
        public virtual ICollection<TrainStation> TrainStation { get; set; } = new List<TrainStation>();
    }
}
