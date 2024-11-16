using FreshInventory.Application.DTO.UserDTO;

namespace FreshInventory.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<LoginUserResponseDto> LoginUserAsync(LoginUserDto loginUserDto);
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<bool> UpdateUserAsync(UpdateUserDto updateUserDto);
    }
}
