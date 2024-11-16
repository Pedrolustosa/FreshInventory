namespace FreshInventory.Application.DTO.UserDTO
{
    public record UpdateUserDto(string Id, string FullName, DateTime DateOfBirth, string Email);
}