namespace Travel.Core.DTOs
{
    public class SearchScheduleRequest
    {
        public Guid TourId { get; set; }
        public DateTime DateStart { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}
