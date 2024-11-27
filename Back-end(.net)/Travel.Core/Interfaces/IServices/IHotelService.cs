using Microsoft.AspNetCore.Http;
using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> GetAll();
        Task<IEnumerable<Hotel>> GetByDestination(int destiantionId);
        Task<IEnumerable<Hotel>> GetByPartner(Guid partnerId);
        Task<Hotel?> GetById(Guid id);
        Task CreateHotel(Hotel hotel);    
        Task<bool> UpdateHotel(Guid id, Hotel hotel);
        Task<bool> UploadImagesAsync(List<IFormFile> files, Guid hotelId);
        Task<PagedResult<Hotel>> SearchHotel(SearchHotelRequest request);
    }
}
