namespace Travel.Core.Entities
{
    public class Image
    {
        public Guid Id { get; set; }

        public string Path { get; set; } = null!;

        public string? Title { get; set; }

        public int? HotelId { get; set; }

        public int? DestinationId { get; set; }

        public int? RoomId { get; set; }

        public int? ActivityId { get; set; }

        public virtual Activity? Activity { get; set; }

        public virtual Destination? Destination { get; set; }

        public virtual Hotel? Hotel { get; set; }

        public virtual Room? Room { get; set; }
    }
}
