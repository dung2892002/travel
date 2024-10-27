using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface ITourRepository
    {
        Task Create(Tour tour);
        Task<Tour?> GetById(Guid id);
        Task<IEnumerable<Tour>> GetAll();
        Task<IEnumerable<Tour>> GetByPartner(Guid partnerId);
        Task<IEnumerable<Tour>> GetByCity(int cityId);
        Task<IEnumerable<Tour>> GetByDestination(int destinationId);
    }
}
