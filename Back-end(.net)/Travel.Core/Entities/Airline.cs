namespace Travel.Core.Entities
{
    public class Airline
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public virtual ICollection<Flight> Flight { get; set; } = new List<Flight>();
    }
}
