namespace Travel.Core.Entities
{
    public class Review
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public short Point { get; set; }

        public string Description { get; set; } = null!;

        public string CreatedAt { get; set; } = null!;

        public int? HotelId { get; set; }

        public int? DestinationId { get; set; }

        public int? TourId { get; set; }

        public virtual Destination? Destination { get; set; }

        public virtual Hotel? Hotel { get; set; }

        public virtual Tour? Tour { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
