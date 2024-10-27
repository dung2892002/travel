using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface ITimeSlotRepository
    {
        Task Create(TimeSlot timeSlot);
    }
}
