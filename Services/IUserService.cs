using Mediar.Models;

namespace Mediar.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(User user);
        Task<string?> LoginUserAsync(string email, string password);
        Task<bool> AdminExists();
        Task<IEnumerable<User>> GetAllUsers();
        Task DeleteUser(int Id);
        Task UpdateUser(User user);
        Task<User?> GetUserAsync(int userId);
    }
}
