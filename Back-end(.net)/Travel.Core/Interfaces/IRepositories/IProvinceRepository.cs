using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IProvinceRepository
    {
        Task<IEnumerable<Province>> GetAll();
        Task<IEnumerable<Province>> GetByName(string key);
    }
}
