namespace Travel.Core.Entities
{
    public class Flight
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public DateTime DepartureTime { get; set; }

        public int NumberEconomicClass { get; set; }

        public int NumberBusinessClass { get; set; }

        public int NumberFirstClass { get; set; }

        public int NumberPremiumEconomyClass { get; set; }

        public int AirlineId { get; set; }

        public int DepartureAirportId { get; set; }

        public int ArrivalAirportId { get; set; }

        public virtual Airline Airline { get; set; } = null!;

        public virtual Airport ArrivalAirport { get; set; } = null!;

        public virtual ICollection<BookingFlight> BookingFlight { get; set; } = new List<BookingFlight>();

        public virtual Airport DepartureAirport { get; set; } = null!;
    }
}
