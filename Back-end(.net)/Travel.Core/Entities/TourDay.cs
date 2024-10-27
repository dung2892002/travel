using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class TourDay
    {
        public Guid Id { get; set; }

        public Guid TourId { get; set; }

        public int DayNumber { get; set; }

        public string Description { get; set; } = null!;

        public virtual ICollection<TimeSlot> TimeSlot { get; set; } = new List<TimeSlot>();

        [JsonIgnore]
        public virtual Tour? Tour { get; set; }
    }
}
