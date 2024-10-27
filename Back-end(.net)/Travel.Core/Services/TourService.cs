using Microsoft.AspNetCore.Http;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class TourService(IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService) : ITourService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;  
        private readonly IFirebaseStorageService _firebaseStorageService = firebaseStorageService;
        public async Task Create(Tour tour)
        {
            try
            {
                tour.Id = Guid.NewGuid();
                await _unitOfWork.Tours.Create(tour);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                // Handle exception (log it, rethrow it, etc.)
                throw new ApplicationException("Error creating tour", ex);
            }
        }

        public async Task<IEnumerable<Tour>> GetAll()
        {
            return await _unitOfWork.Tours.GetAll();
        }

        public async Task<IEnumerable<Tour>> GetByCity(int cityId)
        {
            return await _unitOfWork.Tours.GetByCity(cityId);
        }

        public async Task<IEnumerable<Tour>> GetByDestination(int destinationId)
        {
            return await _unitOfWork.Tours.GetByDestination(destinationId);
        }

        public async Task<Tour?> GetById(Guid id)
        {
            return await _unitOfWork.Tours.GetById(id);
        }

        public async Task<IEnumerable<Tour>> GetByPartner(Guid partnerId)
        {
            return await _unitOfWork.Tours.GetByPartner(partnerId);
        }

        public async Task<bool> UploadImagesAsync(List<IFormFile> files, Guid tourId)
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
                        TourId = tourId,
                    };

                    await _unitOfWork.Images.AddImage(image);
                }
            }
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
