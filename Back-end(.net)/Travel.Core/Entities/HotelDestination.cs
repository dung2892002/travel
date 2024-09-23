namespace Travel.Core.Entities
{
    public class HotelDestination
    {
        public int Id { get; set; }

        public int? HotelId { get; set; }

        public int? DestinationId { get; set; }

        public virtual Destination? Destination { get; set; }

        public virtual Hotel? Hotel { get; set; }
    }
}
