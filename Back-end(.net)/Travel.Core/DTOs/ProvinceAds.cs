namespace Travel.Core.DTOs
{
    public class ProvinceAds
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public int? TotalHotel { get; set; }
        public int? TotalTour { get; set; }
    }
}
