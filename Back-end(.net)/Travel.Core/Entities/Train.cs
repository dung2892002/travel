namespace Travel.Core.Entities
{
    public class Train
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public DateTime DepartureTime { get; set; }

        public int NumberStandardClass { get; set; }

        public int NumberFirstClass { get; set; }

        public int NumberExecutiveClass { get; set; }

        public int NumberBusinessClass { get; set; }

        public int TrainOperatorId { get; set; }

        public int DepartureTrainStationId { get; set; }

        public int ArrivalTrainStationId { get; set; }

        public virtual TrainStation ArrivalTrainStation { get; set; } = null!;

        public virtual ICollection<BookingTrain> BookingTrain { get; set; } = new List<BookingTrain>();

        public virtual TrainStation DepartureTrainStation { get; set; } = null!;

        public virtual TrainOperator TrainOperator { get; set; } = null!;
    }
}
