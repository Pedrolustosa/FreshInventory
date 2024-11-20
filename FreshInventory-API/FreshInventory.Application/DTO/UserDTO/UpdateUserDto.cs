namespace FreshInventory.Application.DTO.UserDTO
{
    public class UpdateUserDto()
    {
        public required string Id { get; set; }
        public required string FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}