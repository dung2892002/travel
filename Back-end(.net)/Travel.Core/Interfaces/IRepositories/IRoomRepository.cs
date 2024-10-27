using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetByHotel(Guid hotelId);
        Task<Room?> GetById(Guid id);
        Task CreateRoom(Room room);
    }
}
