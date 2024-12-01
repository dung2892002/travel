using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Fullname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string AvatarImage { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual ICollection<BookingFlight> BookingFlight { get; set; } = new List<BookingFlight>();

        [JsonIgnore]
        public virtual ICollection<BookingRoom> BookingRoom { get; set; } = new List<BookingRoom>();

        [JsonIgnore]
        public virtual ICollection<BookingTour> BookingTour { get; set; } = new List<BookingTour>();

        [JsonIgnore]
        public virtual ICollection<BookingTrain> BookingTrain { get; set; } = new List<BookingTrain>();

        [JsonIgnore]
        public virtual ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();

        [JsonIgnore]
        public virtual ICollection<Hotel> Hotel { get; set; } = new List<Hotel>();

        [JsonIgnore]
        public virtual ICollection<Tour> Tour { get; set; } = new List<Tour>();

        [JsonIgnore]
        public virtual ICollection<Review> Review { get; set; } = new List<Review>();

        [JsonIgnore]
        public virtual ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();

        [JsonIgnore]
        public virtual ICollection<Discount> Discount { get; set; } = new List<Discount>();
    }
}
