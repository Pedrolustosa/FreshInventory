using FreshInventory.Domain.Entities;

namespace FreshInventory.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> RegisterUserAsync(User user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> UpdateUserAsync(User user);
    }
}