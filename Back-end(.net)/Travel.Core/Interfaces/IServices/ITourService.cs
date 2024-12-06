using Microsoft.AspNetCore.Http;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface ITourService
    {
        Task Create(Tour tour);
        Task<IEnumerable<Tour>> GetAll();
        Task<IEnumerable<Tour>> GetByPartner(Guid partnerId);
        Task<IEnumerable<Tour>> GetByCity(int cityId);
        Task<IEnumerable<Tour>> GetByDepartureCity(int cityId);
        Task<Tour?> GetById(Guid id);
        Task<bool> UploadImagesAsync(List<IFormFile> files, Guid hotelId);
    }
}
