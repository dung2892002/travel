using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IProvinceService
    {
        Task<IEnumerable<Province>> GetAll();
        Task<IEnumerable<Province>> GetByName(string keyword);
        Task<IEnumerable<ProvinceAds>> GetByHotel();
        Task<IEnumerable<ProvinceAds>> GetByTour();
    }
}
