using Microsoft.AspNetCore.Http;
using Travel.Core.DTOs;
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
            await _unitOfWork.BeginTransaction();
            try
            {
                room.Id = Guid.NewGuid();
                await _unitOfWork.Rooms.CreateRoom(room);

                foreach (var roomFacility in room.RoomFacility)
                {
                    roomFacility.RoomId = room.Id;
                    roomFacility.Facility = null;
                    await _unitOfWork.Rooms.CreateRoomFacility(roomFacility);
                }
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                throw new ApplicationException("Error creating hotel", ex);
            }
        }

        public async Task<IEnumerable<Room>> GetRoomByHotel(Guid hotelId)
        {
            var hotelExisting = await _unitOfWork.Hotels.GetById(hotelId);
            if (hotelExisting == null)
            {
                throw new ArgumentException("Hotel not exits");
            }

            var rooms = await _unitOfWork.Rooms.GetByHotel(hotelId);
            return rooms;
        }

        public async Task<Room?> GetRoomDetail(Guid id)
        {
            var room = await _unitOfWork.Rooms.GetDetail(id);
            if (room == null)
            {
                throw new ArgumentException("Room not exits");
            }
            return room;
        }

        public async Task<IEnumerable<SearchRoomResponse>> SearchRoom(SearchRoomRequest request)
        {
            var rooms = await _unitOfWork.Rooms.SearchRoom(request);
            var respones = new List<SearchRoomResponse>();
            foreach (var room in rooms)
            {
                var availableRoom = room.Quantity - room.BookingRoom.Where(b => b.Status != 2 && ((b.CheckInDate < request.CheckOut && b.CheckOutDate > request.CheckIn)))
                                                      .Sum(b => b.Quantity);

                var roomResponse = new SearchRoomResponse
                {
                    Id = room.Id,
                    Name = room.Name,
                    Price = room.Price,
                    Quantity = room.Quantity,
                    Area = room.Area,
                    MaxAdultPeople = room.MaxAdultPeople,
                    MaxChildrenPeople = room.MaxChildrenPeople,
                    SingleBed = room.SingleBed,
                    DoubleBed = room.DoubleBed,
                    Dirention = room.Dirention,
                    HotelId = room.HotelId,
                    Image = room.Image,
                    RoomFacility = room.RoomFacility,
                    AvailableQuantity = (short)availableRoom,
                };

                respones.Add(roomResponse);
            }
            return respones;
        }

        public async Task<bool> UpdateRoom(Guid roomId, Room room)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                var roomExisting = await _unitOfWork.Rooms.GetDetail(roomId);
                if (roomExisting == null)
                {
                    throw new ArgumentException("Room not exist");
                }

                roomExisting.Name = room.Name;
                roomExisting.Price = room.Price;
                roomExisting.Quantity = room.Quantity;
                roomExisting.Area = room.Area;
                roomExisting.MaxAdultPeople = room.MaxAdultPeople;
                roomExisting.MaxChildrenPeople = room.MaxChildrenPeople;
                roomExisting.SingleBed = room.SingleBed;
                roomExisting.DoubleBed = room.DoubleBed;
                roomExisting.Dirention = room.Dirention;

                UpdateRoomFacilities(roomExisting, room.RoomFacility);

                var result = await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransaction();

                return result > 0;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                await _unitOfWork.RollbackTransaction();
                throw new ApplicationException("Error update room", ex);
            }
        }

        public async Task<bool> UploadImagesAsync(List<IFormFile> files, Guid roomId)
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

        private async void UpdateRoomFacilities(Room existingRoom, ICollection<RoomFacility> newFacilities)
        {
            var roomFacilityRemove = existingRoom.RoomFacility
                .Where(f => !newFacilities.Any(nf => nf.FacilityId == f.FacilityId))
                .ToList();
            foreach (var roomFacility in roomFacilityRemove)
            {
                await _unitOfWork.Rooms.DeleteRoomFacility(roomFacility);
            }

            var existingIds = existingRoom.RoomFacility.Select(f => f.FacilityId).ToList();
            var roomFacilityAdd = newFacilities.Where(nf => !existingIds.Contains(nf.FacilityId)).ToList();
            foreach (var roomFacility in roomFacilityAdd)
            {
                roomFacility.RoomId = existingRoom.Id;
                await _unitOfWork.Rooms.CreateRoomFacility(roomFacility);
            }
        }
    }
}
