using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByRoleValueAsync(int roleValue);
    }
}
