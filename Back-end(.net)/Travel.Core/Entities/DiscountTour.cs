using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class DiscountTour
    {
        public int Id { get; set; }
        public Guid DiscountId { get; set; }
        public Guid TourId { get; set; }
        [JsonIgnore]
        public Discount? Discount { get; set; }
        [JsonIgnore]
        public Tour? Tour { get; set; }
    }
}
