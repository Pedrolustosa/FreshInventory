using FreshInventory.Application.DTO.UserDTO;

namespace FreshInventory.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserReadDto> RegisterUserAsync(UserCreateDto registerUserDto);
        Task<UserLoginResponseDto> LoginUserAsync(UserLoginDto loginUserDto);
        Task<UserReadDto> GetUserByEmailAsync(string email);
        Task<UserReadDto> GetUserByIdAsync(Guid userId);
        Task<UserReadDto> UpdateUserAsync(Guid userId, UserUpdateDto updateUserDto);
    }
}
