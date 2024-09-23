namespace Travel.Core.Entities
{
    public class TourDay
    {
        public int Id { get; set; }

        public int TourId { get; set; }

        public int DayNumber { get; set; }

        public string Description { get; set; } = null!;

        public virtual ICollection<TimeSlot> TimeSlot { get; set; } = new List<TimeSlot>();

        public virtual Tour Tour { get; set; } = null!;
    }
}
