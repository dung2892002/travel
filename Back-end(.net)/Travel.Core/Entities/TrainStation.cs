namespace Travel.Core.Entities
{
    public class TrainStation
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int CityId { get; set; }

        public virtual City City { get; set; } = null!;

        public virtual ICollection<Train> TrainArrivalTrainStation { get; set; } = new List<Train>();

        public virtual ICollection<Train> TrainDepartureTrainStation { get; set; } = new List<Train>();
    }
}
