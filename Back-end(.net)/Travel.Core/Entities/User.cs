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

        public virtual ICollection<BookingFlight> BookingFlight { get; set; } = new List<BookingFlight>();

        public virtual ICollection<BookingRoom> BookingRoom { get; set; } = new List<BookingRoom>();

        public virtual ICollection<BookingTour> BookingTour { get; set; } = new List<BookingTour>();

        public virtual ICollection<BookingTrain> BookingTrain { get; set; } = new List<BookingTrain>();

        public virtual ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();

        public virtual ICollection<Hotel> Hotel { get; set; } = new List<Hotel>();

        public virtual ICollection<Review> Review { get; set; } = new List<Review>();

        public virtual ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
    }
}
