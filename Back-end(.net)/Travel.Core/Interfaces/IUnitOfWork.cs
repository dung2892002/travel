using Travel.Core.Interfaces.IRepositories;

namespace Travel.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IUserRoleRepository UserRoles { get; }

        Task<int> CompleteAsync();
        void Dispose();
    }
}
