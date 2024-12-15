using Travel.Core.Entities;

namespace Travel.Core.DTOs
{
    public class SearchTourResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Rating { get; set; }
        public string Transport { get; set; } = null!;
        public int NumberOfDay { get; set; }
        public int NumberOfNight { get; set; }
        public double AverageReview { get; set; }
        public int QuantityReview { get; set; }
        public decimal MinPrice { get; set; }
        public City? DepartureCity { get; set; }
        public virtual ICollection<Image> Image { get; set; } = new List<Image>();
    }
}
