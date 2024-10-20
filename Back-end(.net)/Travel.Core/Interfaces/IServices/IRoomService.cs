using Microsoft.AspNetCore.Http;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetRoomByHotel(int hotelId);
        Task CreateRoom(Room room);
        Task<bool> UpdateRoom(int roomId, Room room);
        Task<bool> UploadImagesAsync(List<IFormFile> files, int roomId);

    }
}
