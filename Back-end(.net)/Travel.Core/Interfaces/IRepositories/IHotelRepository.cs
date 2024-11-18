using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllHotel();

        Task<Hotel?> GetById(Guid id);

        Task<IEnumerable<Hotel>> GetByPartner(Guid partnerId);

        Task<IEnumerable<Hotel>> GetByDestination(int destinationId);

        Task CreateHotel(Hotel hotel);
    }
}
