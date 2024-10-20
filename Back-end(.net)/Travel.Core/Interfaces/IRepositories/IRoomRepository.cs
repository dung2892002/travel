using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetByHotel(int hotelId);
        Task<Room?> GetById(int id);
        Task CreateRoom(Room room);
    }
}
