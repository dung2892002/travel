using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface ITourDayRepository
    {
        Task Create(TourDay tourDay);
    }
}
