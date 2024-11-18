using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class Room
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public short Quantity { get; set; }

        public decimal Area { get; set; }

        public bool FreeWifi { get; set; }

        public short MaxAdultPeople { get; set; }

        public short MaxChildrenPeople { get; set; }

        public short SingleBed { get; set; }

        public short DoubleBed { get; set; }

        public string? Dirention { get; set; }

        public Guid HotelId { get; set; }

        [JsonIgnore]
        public virtual ICollection<BookingRoom> BookingRoom { get; set; } = new List<BookingRoom>();

        [JsonIgnore]
        public virtual Hotel? Hotel { get; set; } 

        public virtual ICollection<Image> Image { get; set; } = new List<Image>();
        public virtual ICollection<RoomFacility> RoomFacility { get; set; } = new List<RoomFacility>();
    }
}
