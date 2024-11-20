namespace FreshInventory.Application.DTO.UserDTO
{
    public class RegisterUserDto()
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
