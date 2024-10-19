using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public short Quantity { get; set; }

        public decimal Area { get; set; }

        public bool FreeWifi { get; set; }

        public short MaxAdultPeople { get; set; }

        public short MaxChildrenPeople { get; set; }

        public bool NoSmoking { get; set; }

        public short SingleBed { get; set; }

        public short DoubleBed { get; set; }

        public bool HasWindow { get; set; }

        public bool HasBathub { get; set; }

        public bool HasAirConditioned { get; set; }

        public string? Dirention { get; set; }

        public int HotelId { get; set; }

        [JsonIgnore]
        public virtual ICollection<BookingRoom> BookingRoom { get; set; } = new List<BookingRoom>();

        [JsonIgnore]
        public virtual Hotel Hotel { get; set; } = null!;

        public virtual ICollection<Image> Image { get; set; } = new List<Image>();
    }
}
