using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAll();
        Task<IEnumerable<City>> GetByNameContain(string name);
        Task<IEnumerable<City>> GetByNameProvince(int provinceId);
    }
}
