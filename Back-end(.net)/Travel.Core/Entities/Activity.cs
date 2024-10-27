using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class Activity
    {
        public Guid Id { get; set; }

        public Guid TimeSlotId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Location { get; set; } = null!;
                
        [JsonIgnore]
        public virtual TimeSlot? TimeSlot { get; set; } 
    }
}
