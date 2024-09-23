namespace Travel.Core.Entities
{
    public class Activity
    {
        public int Id { get; set; }

        public int TimeSlotId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Location { get; set; } = null!;

        public virtual ICollection<Image> Image { get; set; } = new List<Image>();

        public virtual TimeSlot TimeSlot { get; set; } = null!;
    }
}
