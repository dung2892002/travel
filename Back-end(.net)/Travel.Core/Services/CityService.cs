using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class CityService(IUnitOfWork unitOfWork) : ICityService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public Task<IEnumerable<City>> GetAll()
        {
            return _unitOfWork.Cities.GetAll();
        }

        public Task<IEnumerable<City>> GetByName(string name)
        {
            return _unitOfWork.Cities.GetByNameContain(name);
        }

        public Task<IEnumerable<City>> GetByProvinceId(int provinceId)
        {
            return _unitOfWork.Cities.GetByNameProvince(provinceId);
        }
    }
}
