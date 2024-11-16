namespace FreshInventory.Application.DTO.UserDTO
{
    public record RegisterUserDto(string FullName, string Email, DateTime DateOfBirth, string Password);
}
