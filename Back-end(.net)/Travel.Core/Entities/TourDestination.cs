namespace Travel.Core.Entities
{
    public class TourDestination
    {
        public int Id { get; set; }

        public int TourId { get; set; }

        public int DestinationId { get; set; }

        public int VisitOrder { get; set; }

        public virtual Destination Destination { get; set; } = null!;

        public virtual Tour Tour { get; set; } = null!;
    }
}
