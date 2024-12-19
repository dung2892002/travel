namespace Travel.Core.DTOs
{
    public class StatisticalRequest
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? TourId { get; set; }
        public Guid? UserId { get; set; }
    }
}
