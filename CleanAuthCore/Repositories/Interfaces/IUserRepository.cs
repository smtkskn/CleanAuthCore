using CleanAuthCore.Entities;

namespace CleanAuthCore.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserNameAsync(string username);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task<bool> ValidateUserAsync(string username, string password);
    }
}
