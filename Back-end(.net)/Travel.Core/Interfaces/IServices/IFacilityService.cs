using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IFacilityService
    {
        Task<IEnumerable<Facility>> GetFacilities(int type);
    }
}
