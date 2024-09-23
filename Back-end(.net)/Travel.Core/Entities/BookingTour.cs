namespace Travel.Core.Entities
{
    public class BookingTour
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int TourId { get; set; }

        public DateOnly StartDate { get; set; }

        public short NumberToddler { get; set; }

        public short NumberPrimaryChildren { get; set; }

        public short NumberAdultPeople { get; set; }

        public DateOnly CreatedAt { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; }

        public string CancelReason { get; set; } = null!;

        public virtual Tour Tour { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
