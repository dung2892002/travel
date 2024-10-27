namespace Travel.Core.Entities
{
    public class TourDestination
    {
        public Guid Id { get; set; }

        public Guid TourId { get; set; }

        public int DestinationId { get; set; }

        public int VisitOrder { get; set; }

        public virtual Destination? Destination { get; set; }

        public virtual Tour? Tour { get; set; }
    }
}
