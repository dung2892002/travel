namespace Travel.Core.Entities
{
    public class TimeSlot
    {
        public int Id { get; set; }

        public int TourDayId { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string Description { get; set; } = null!;

        public virtual ICollection<Activity> Activity { get; set; } = new List<Activity>();

        public virtual TourDay TourDay { get; set; } = null!;
    }
}
