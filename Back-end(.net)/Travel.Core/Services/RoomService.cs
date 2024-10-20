using Microsoft.AspNetCore.Http;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class RoomService(IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService) : IRoomService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService = firebaseStorageService;
        public async Task CreateRoom(Room room)
        {
            var hotelExisting = await _unitOfWork.Hotels.GetById(room.HotelId);
            if (hotelExisting == null)
            {
                throw new ArgumentException("Hotel not exits");
            }
            await _unitOfWork.Room.CreateRoom(room);
        }

        public async Task<IEnumerable<Room>> GetRoomByHotel(int hotelId)
        {
            var hotelExisting = await _unitOfWork.Hotels.GetById(hotelId);
            if (hotelExisting == null)
            {
                throw new ArgumentException("Hotel not exits");
            }

            var rooms = await _unitOfWork.Room.GetByHotel(hotelId);
            return rooms;
        }

        public async Task<bool> UpdateRoom(int roomId, Room room)
        {
            var roomExisting = await _unitOfWork.Room.GetById(roomId);
            if(roomExisting == null)
            {
                throw new ArgumentException("Room not exist");
            }

            roomExisting.Name = room.Name;
            roomExisting.Price = room.Price;
            roomExisting.Quantity = room.Quantity;
            roomExisting.Area = room.Area;
            roomExisting.FreeWifi = room.FreeWifi;
            roomExisting.MaxAdultPeople = room.MaxAdultPeople;
            roomExisting.MaxChildrenPeople = room.MaxChildrenPeople;
            roomExisting.NoSmoking = room.NoSmoking;
            roomExisting.SingleBed = room.SingleBed;
            roomExisting.DoubleBed = room.DoubleBed;
            roomExisting.HasWindow = room.HasWindow;
            roomExisting.HasBathub = room.HasBathub;
            roomExisting.HasAirConditioned = room.HasAirConditioned;
            roomExisting.Dirention = room.Dirention;

            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }

        public async Task<bool> UploadImagesAsync(List<IFormFile> files, int roomId)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("No files were provided.");
            var filePaths = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var path = DateTime.Now.ToString("dd-MM-yyyy") + "_" + Guid.NewGuid().ToString() + "_" + file.FileName;
                    using Stream stream = file.OpenReadStream();
                    var result = await _firebaseStorageService.UploadFile(stream, path);
                    if (!result) throw new Exception("Upload image error");

                    var image = new Image
                    {
                        Path = path,
                        RoomId = roomId,
                    };

                    await _unitOfWork.Images.AddImage(image);
                }
            }
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
