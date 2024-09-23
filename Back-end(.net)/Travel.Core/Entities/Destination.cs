namespace Travel.Core.Entities
{
    public class Destination
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Descrption { get; set; } = null!;

        public int CityId { get; set; }

        public virtual City City { get; set; } = null!;

        public virtual ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();

        public virtual ICollection<HotelDestination> HotelDestination { get; set; } = new List<HotelDestination>();

        public virtual ICollection<Image> Image { get; set; } = new List<Image>();

        public virtual ICollection<Review> Review { get; set; } = new List<Review>();

        public virtual ICollection<TourDestination> TourDestination { get; set; } = new List<TourDestination>();
    }
}
