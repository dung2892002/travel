using Microsoft.AspNetCore.Http;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetRoomByHotel(Guid hotelId);
        Task CreateRoom(Room room);
        Task<bool> UpdateRoom(Guid roomId, Room room);
        Task<bool> UploadImagesAsync(List<IFormFile> files, Guid roomId);

    }
}
