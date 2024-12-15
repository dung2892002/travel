namespace Travel.Core.DTOs
{
    public class SearchTourRequest
    {
        public int CityId { get; set; }
        public int PageNumber { get; set; }
        public DateTime? DateStart { get; set; }
        public List<int>? GuestRatings { get; set; }
        public List<int>? Ratings { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}
