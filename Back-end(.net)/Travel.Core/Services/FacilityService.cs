using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class FacilityService(IUnitOfWork unitOfWork) : IFacilityService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<IEnumerable<Facility>> GetFacilities(int type)
        {
            var facilities = await _unitOfWork.Facilities.GetFacility(type);
            return facilities;
        }
    }
}
