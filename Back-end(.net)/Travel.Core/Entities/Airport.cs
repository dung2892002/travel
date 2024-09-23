namespace Travel.Core.Entities
{
    public class Airport
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int CityId { get; set; }

        public virtual City City { get; set; } = null!;

        public virtual ICollection<Flight> FlightArrivalAirport { get; set; } = new List<Flight>();

        public virtual ICollection<Flight> FlightDepartureAirport { get; set; } = new List<Flight>();
    }
}
