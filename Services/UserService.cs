using Mediar.Models;
using Mediar.Repositories;

namespace Mediar.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task RegisterUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists!");

            user.Password = _authService.HashPassword(user.Password);

            await _userRepository.CreateAsync(user);
        }


        public async Task<string?> LoginUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return null;
            
            if (!_authService.ValidatePassword(password, user.Password))
                return null;

            return _authService.GenerateJWTToken(user.Id, user.Name, user.Email, user.Permission);
        }

        public async Task<bool> AdminExists()
        {
            return await _userRepository.AdminExistsAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task DeleteUser(int Id)
        {
            await _userRepository.DeleteAsync(Id);
        }

        public async Task UpdateUser(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }
    }
}
