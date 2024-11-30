namespace FreshInventory.Application.DTO.UserDTO
{
    public record UserDto(Guid Id, string FullName, string Email, DateTime DateOfBirth);
}
