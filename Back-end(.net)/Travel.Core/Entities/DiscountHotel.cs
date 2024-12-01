using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class DiscountHotel
    {
        public int Id { get; set; }
        public Guid DiscountId { get; set; }
        public Guid HotelId { get; set; }

        [JsonIgnore]
        public Hotel? Hotel { get; set; }
        [JsonIgnore]
        public Discount? Discount { get; set; }
    }
}
