using System;
using System.Threading.Tasks;
using WalletKata.Models;
using WalletKata.Repositories.Interfaces;
using WalletKata.Services.Interfaces;

namespace WalletKata.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IRepository<User> userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
             _unitOfWork = unitOfWork;
                
        }

        public async Task<long> CreateUser(string username)
        {
            _unitOfWork.BeginTransaction();

            // Validate input
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be empty");

            // Check if user already exists
            var existingUser = await _userRepository.GetAllAsync();
            if (existingUser.Any(u => u.Name == username))
                throw new ArgumentException("User already exists");

            // Create new user
            var user = new User { Name = username };
            var newUser = await _userRepository.AddAsync(user);
            return newUser.UserId;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                // Handle exception or log it
                throw new Exception("Error occurred while retrieving users.", ex);
            }
        }
    }
}
