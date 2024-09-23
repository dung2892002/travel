using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task AddAsync(User user);
    }
}
