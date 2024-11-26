using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserById(Guid id);
        Task<User?> GetDetailUser(Guid id);
        Task Add(User user);
    }
}
