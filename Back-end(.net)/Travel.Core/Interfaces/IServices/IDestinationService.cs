using Microsoft.AspNetCore.Http;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IDestinationService 
    {
        Task<IEnumerable<Destination>> GetByCity(int cityId);

        Task<bool> UploadImagesAsync(List<IFormFile> files, int destinationId);
    }
}
