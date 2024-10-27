using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class TimeSlot
    {
        public Guid Id { get; set; }

        public Guid TourDayId { get; set; }

        public int Time {  get; set; }

        public string Description { get; set; } = null!;

        public virtual ICollection<Activity> Activity { get; set; } = new List<Activity>();

        [JsonIgnore]
        public virtual TourDay? TourDay { get; set; }
    }
}
