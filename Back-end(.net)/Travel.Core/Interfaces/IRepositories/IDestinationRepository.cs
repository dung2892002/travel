using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IDestinationRepository
    {
        Task<IEnumerable<Destination>> GetByCity(int cityId);
    }
}
