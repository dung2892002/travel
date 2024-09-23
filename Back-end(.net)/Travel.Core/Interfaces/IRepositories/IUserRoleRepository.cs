using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IUserRoleRepository
    {
        Task AddAsync(UserRole userRole);
    }
}
