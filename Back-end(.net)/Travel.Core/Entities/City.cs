namespace Travel.Core.Entities
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int CountryId { get; set; }

        public string Image { get; set; } = null!;

        public virtual ICollection<Airport> Airport { get; set; } = new List<Airport>();

        public virtual Country Country { get; set; } = null!;

        public virtual ICollection<Destination> Destination { get; set; } = new List<Destination>();

        public virtual ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();

        public virtual ICollection<Tour> Tour { get; set; } = new List<Tour>();

        public virtual ICollection<TrainStation> TrainStation { get; set; } = new List<TrainStation>();
    }
}
