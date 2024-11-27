namespace Travel.Core.DTOs
{
    public class SearchRoomRequest
    {
            public Guid HotelId { get; set; }
            public DateTime CheckIn { get; set; }
            public DateTime CheckOut { get; set; }
            public int QuantityAdultPeople { get; set; }
            public int QuantityChildrenPeople { get; set; }
            public int QuantityRoom { get; set; }
            public int? MinPrice { get; set; }
            public int? MaxPrice { get; set; }

    }
}
