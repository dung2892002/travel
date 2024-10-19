using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class ProvinceService(IUnitOfWork unitOfWork) : IProvinceService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<IEnumerable<Province>> GetAll()
        {
            return await _unitOfWork.Provinces.GetAll();
        }

        public async Task<IEnumerable<Province>> GetByName(string keyword)
        {
            return await _unitOfWork.Provinces.GetByName(keyword);
        }
    }
}
