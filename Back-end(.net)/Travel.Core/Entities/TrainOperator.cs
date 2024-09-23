namespace Travel.Core.Entities
{
    public class TrainOperator
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public virtual ICollection<Train> Train { get; set; } = new List<Train>();
    }
}
