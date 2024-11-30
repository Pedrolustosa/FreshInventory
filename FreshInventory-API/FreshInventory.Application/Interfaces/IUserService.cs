using FreshInventory.Application.DTO.UserDTO;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<LoginUserResponseDto> LoginUserAsync(LoginUserDto loginUserDto);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<bool> UpdateUserAsync(UpdateUserDto updateUserDto);
    }
}
