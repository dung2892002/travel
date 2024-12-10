namespace Travel.Core.Entities;

public class Payment
{
    public Guid Id { get; set; }

    public Guid? BookingRoomId { get; set; }

    public Guid? BookingTourId { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public long TransactionId { get; set; }

    public virtual BookingRoom? BookingRoom { get; set; }

    public virtual BookingTour? BookingTour { get; set; }
}
