namespace Travel.Core.Entities
{
    public class Hotel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Rating { get; set; }

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public TimeOnly CheckInTime { get; set; }

        public TimeOnly CheckOutTime { get; set; }

        public bool AllowedAnimal { get; set; }

        public Guid UserId { get; set; }

        public virtual ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();

        public virtual ICollection<HotelDestination> HotelDestination { get; set; } = new List<HotelDestination>();

        public virtual ICollection<Image> Image { get; set; } = new List<Image>();

        public virtual ICollection<Review> Review { get; set; } = new List<Review>();

        public virtual ICollection<Room> Room { get; set; } = new List<Room>();

        public virtual User User { get; set; } = null!;
    }
}
