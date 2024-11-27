using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetByHotel(Guid hotelId);
        Task<Room?> GetById(Guid id);
        Task CreateRoom(Room room);
        Task<Room?> GetDetail(Guid id);
        Task CreateRoomFacility(RoomFacility roomFacility);
        Task DeleteRoomFacility(RoomFacility roomFacility);
        Task<IEnumerable<Room>> SearchRoom(SearchRoomRequest request);
    }
}
 