using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class HotelService(IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService) : IHotelService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService = firebaseStorageService;

        
        public async Task CreateHotel(Hotel hotel)
        {
            try
            {
                hotel.Id = Guid.NewGuid();
                await _unitOfWork.Hotels.CreateHotel(hotel);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                // Handle exception (log it, rethrow it, etc.)
                throw new ApplicationException("Error creating hotel", ex);
            }
        }

        public async Task<IEnumerable<Hotel>> GetAll()
        {
            return await _unitOfWork.Hotels.GetAllHotel();
        }

        public async Task<IEnumerable<Hotel>> GetByDestination(int destiantionId)
        {
            return await _unitOfWork.Hotels.GetByDestination(destiantionId);
        }

        public async Task<Hotel?> GetById(Guid id)
        {
            var hotel = await _unitOfWork.Hotels.GetById(id);
            if (hotel == null)
            {
                throw new ArgumentException("Hotel not exist");
            }
            return hotel;
        }

        public async Task<IEnumerable<Hotel>> GetByPartner(Guid partnerId)
        {
            return await _unitOfWork.Hotels.GetByPartner(partnerId);
        }

        public async Task<bool> UpdateHotel(Guid id, Hotel hotel)
        {
            var hotelExisting = await _unitOfWork.Hotels.GetById(id);
            if (hotelExisting == null)
            {
                throw new ArgumentException("Hotel not exist");
            }

            Console.WriteLine($"name truowsc khi doi: {hotelExisting.Name}");
            hotelExisting.Name = hotel.Name;
            Console.WriteLine($"name sau khi doi: {hotelExisting.Name}");

            hotelExisting.Description = hotel.Description;
            hotelExisting.Rating = hotel.Rating;
            hotelExisting.CheckInTime = hotel.CheckInTime;
            hotelExisting.CheckOutTime = hotel.CheckOutTime;

            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }

        public async Task<bool> UploadImagesAsync(List<IFormFile> files, Guid hotelId)
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
                        HotelId = hotelId,
                    };

                    await _unitOfWork.Images.AddImage(image);
                }
            }
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
