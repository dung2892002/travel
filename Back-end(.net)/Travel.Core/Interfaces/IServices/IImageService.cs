using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IImageService
    {
        Task AddImage(Image image);

        Task<IEnumerable<Image>> GetImageOfDestination(int destinationId);
    }
}
