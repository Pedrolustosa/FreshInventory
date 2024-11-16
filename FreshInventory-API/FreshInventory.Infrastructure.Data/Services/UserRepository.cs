using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Exceptions;

namespace FreshInventory.Infrastructure.Data.Services
{
    public class UserRepository(UserManager<User> userManager, ILogger<UserRepository> logger) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ILogger<UserRepository> _logger = logger;

        public async Task<User> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", userId);
                    throw new RepositoryException($"User with ID {userId} not found.");
                }
                _logger.LogInformation("User with ID {UserId} retrieved successfully.", userId);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user with ID {UserId}.", userId);
                throw new RepositoryException("An error occurred while retrieving the user.", ex);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("User with email {Email} not found.", email);
                    throw new RepositoryException($"User with email {email} not found.");
                }
                _logger.LogInformation("User with email {Email} retrieved successfully.", email);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user with email {Email}.", email);
                throw new RepositoryException("An error occurred while retrieving the user by email.", ex);
            }
        }

        public async Task<bool> RegisterUserAsync(User user, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("User registration failed for {UserName}. Errors: {Errors}", user.UserName, string.Join(", ", result.Errors.Select(e => e.Description)));
                    throw new RepositoryException("User registration failed.");
                }
                _logger.LogInformation("User {UserName} registered successfully.", user.UserName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering user {UserName}.", user.UserName);
                throw new RepositoryException("An error occurred while registering the user.", ex);
            }
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            try
            {
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
                if (!isPasswordValid)
                {
                    _logger.LogWarning("Invalid password for user {UserName}.", user.UserName);
                }
                return isPasswordValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking password for user {UserName}.", user.UserName);
                throw new RepositoryException("An error occurred while checking the user's password.", ex);
            }
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

    }
}
