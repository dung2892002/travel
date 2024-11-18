using Microsoft.AspNetCore.Http;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IDestinationService 
    {
        Task<IEnumerable<Destination>> GetByCity(int cityId);
        Task<IEnumerable<Destination>> GetAll();
        Task<bool> UploadImagesAsync(List<IFormFile> files, int destinationId);
    }
}
