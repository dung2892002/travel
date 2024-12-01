using Travel.Core.Entities;

namespace Travel.Core.DTOs
{
    public class SearchHotelResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public int Rating { get; set; }
        public int Type { get; set; }

        public double AverageReview {  get; set; }
        public int QuantityReview { get; set; }
        public decimal MinPrice { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string ProvinceName { get; set; } = string.Empty;

        public virtual ICollection<HotelFacility> HotelFacility { get; set; } = new List<HotelFacility>();
        public virtual ICollection<Image> Image { get; set; } = new List<Image>();
    }
}
