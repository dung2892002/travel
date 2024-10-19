using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetByDestination(int destinationId);
        Task AddImage(Image image);
    }
}
