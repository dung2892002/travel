namespace Travel.Core.Entities
{
    public class Tour
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public short NumberOfDay { get; set; }

        public short NumberOfNight { get; set; }

        public short Rating { get; set; }

        public string Description { get; set; } = null!;

        public int DepartureCityId { get; set; }

        /// <summary>
        /// tre duoi 5 tuoi
        /// </summary>
        public decimal PriceToddler { get; set; }

        /// <summary>
        /// tre duoi 10 tuoi
        /// </summary>
        public decimal PricePrimaryChildren { get; set; }

        /// <summary>
        /// tu 10 tuoi tro len
        /// </summary>
        public decimal PriceAdultPeople { get; set; }

        public virtual ICollection<BookingTour> BookingTour { get; set; } = new List<BookingTour>();

        public virtual City DepartureCity { get; set; } = null!;

        public virtual ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();

        public virtual ICollection<Review> Review { get; set; } = new List<Review>();

        public virtual ICollection<TourDay> TourDay { get; set; } = new List<TourDay>();

        public virtual ICollection<TourDestination> TourDestination { get; set; } = new List<TourDestination>();
    }
}
