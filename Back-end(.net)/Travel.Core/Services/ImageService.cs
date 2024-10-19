using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class ImageService(IImageRepository imageRepository) : IImageService, IService
    {
        private readonly IImageRepository _imageRepository = imageRepository;
        public async Task AddImage(Image image)
        {
            await _imageRepository.AddImage(image);
        }

        public async Task<IEnumerable<Image>> GetImageOfDestination(int destinationId)
        {
            return await _imageRepository.GetByDestination(destinationId);
        }
    }
}
