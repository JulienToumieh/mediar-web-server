using Mediar.Models;

namespace Mediar.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int userId);
        Task<string?> GetPasswordAsync(string email);
        Task<bool> AdminExistsAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
