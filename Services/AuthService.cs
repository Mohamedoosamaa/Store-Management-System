using StoreManagementSystem.Models;
using StoreManagementSystem.Repositories;

namespace StoreManagementSystem.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;

        public AuthService()
        {
            _userRepository = new UserRepository();
        }

        // ================= TEST CONNECTION =================

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                await _userRepository.LoginAsync(
                    "test",
                    "test");

                return true;
            }
            catch
            {
                return false;
            }
        }

        // ================= LOGIN =================

        public async Task<User?> LoginAsync(
            string username,
            string password)
        {
            return await _userRepository
                .LoginAsync(username, password);
        }
    }
}