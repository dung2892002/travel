namespace Travel.Core.DTOs
{
    public class SearchHotelRequest
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int QuantityAdultPeople { get; set; }
        public int QuantityChildrenPeople { get; set; }
        public int QuantityRoom { get; set; }
        public int PageNumber { get; set; }
        public int? CityId { get; set; }
        public int? ProvinceId { get; set; }
        public List<int>? HotelFacilities { get; set; }
        public List<int>? GuestRatings { get; set; } 
        public List<int>? Ratings { get; set; }
        public List<int>? Types { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }

    }
}
