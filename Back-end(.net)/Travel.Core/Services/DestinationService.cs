using Microsoft.AspNetCore.Http;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class DestinationService(IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService) : IDestinationService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService = firebaseStorageService;

        public async Task<IEnumerable<Destination>> GetAll()
        {
            return await _unitOfWork.Destinations.GetAll();
        }

        public async Task<IEnumerable<Destination>> GetByCity(int cityId)
        {
            return await _unitOfWork.Destinations.GetByCity(cityId);
        }

        public async Task<bool> UploadImagesAsync(List<IFormFile> files, int destinationId)
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
                        DestinationId = destinationId
                    };

                    await _unitOfWork.Images.AddImage(image);
                }
            }
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
