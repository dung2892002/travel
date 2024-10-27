using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IActivityRepository
    {
        Task Create(Activity activity);
    }
}
