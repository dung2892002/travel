using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class TourCity
    {
        public int Id { get; set; }

        public Guid TourId { get; set; }

        public int CityId { get; set; }

        public int VisitOrder { get; set; }

        public virtual City? City { get; set; }

        [JsonIgnore]
        public virtual Tour? Tour { get; set; }
    }
}
