using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface ICityService
    {
        Task<IEnumerable<City>> GetAll();
        Task<IEnumerable<City>> GetByName(string name);
        Task<IEnumerable<City>> GetByProvinceId(int provinceId);
    }
}
