namespace Travel.Core.Entities
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Continent { get; set; } = null!;

        public string Region { get; set; } = null!;

        public virtual ICollection<City> City { get; set; } = new List<City>();
    }
}
