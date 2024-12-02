using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Exceptions;

namespace FreshInventory.Infrastructure.Data.Services;

public class UserRepository(UserManager<User> userManager, ILogger<UserRepository> logger) : IUserRepository, IDisposable
{
    private bool _disposed = false;
    private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly ILogger<UserRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
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
            _logger.LogError(ex, "Error retrieving user with ID {UserId}: {Message}", userId, ex.Message);
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
            _logger.LogError(ex, "Error retrieving user with email {Email}: {Message}", email, ex.Message);
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
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("User registration failed for {UserName}. Errors: {Errors}", user.UserName, errors);
                throw new RepositoryException("User registration failed.");
            }
            _logger.LogInformation("User {UserName} registered successfully.", user.UserName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user {UserName}: {Message}", user.UserName, ex.Message);
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
            _logger.LogError(ex, "Error checking password for user {UserName}: {Message}", user.UserName, ex.Message);
            throw new RepositoryException("An error occurred while checking the user's password.", ex);
        }
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        try
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to update user {UserName}. Errors: {Errors}", user.UserName, errors);
                throw new RepositoryException("Failed to update the user.");
            }
            _logger.LogInformation("User {UserName} updated successfully.", user.UserName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserName}: {Message}", user.UserName, ex.Message);
            throw new RepositoryException("An error occurred while updating the user.", ex);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _userManager?.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
