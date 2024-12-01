using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class Hotel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Rating { get; set; }

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public TimeOnly CheckInTime { get; set; }

        public TimeOnly CheckOutTime { get; set; }
        public int Type { get; set; }

        public Guid UserId { get; set; }

        public int CityId { get; set; }

        [JsonIgnore]
        public virtual ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();

        [JsonIgnore]
        public virtual ICollection<DiscountHotel> DiscountHotel { get; set; } = new List<DiscountHotel>();

        public virtual ICollection<HotelDestination> HotelDestination { get; set; } = new List<HotelDestination>();

        public virtual ICollection<HotelFacility> HotelFacility { get; set; } = new List<HotelFacility>();

        public virtual ICollection<Review> Review { get; set; } = new List<Review>();


        [JsonIgnore]
        public virtual ICollection<Room> Room { get; set; } = new List<Room>();

        [JsonIgnore]
        public virtual User? User { get; set; }

        public virtual City? City { get; set; }

        public virtual ICollection<Image> Image { get; set; } = new List<Image>();
    }
}
