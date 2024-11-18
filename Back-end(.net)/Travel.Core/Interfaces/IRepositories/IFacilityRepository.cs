using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IFacilityRepository
    {
        Task<IEnumerable<Facility>> GetFacility(int type);
    }
}
