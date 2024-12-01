namespace FreshInventory.Application.DTO.UserDTO
{
    public class UserLoginResponseDto
    {
        public string Token { get; set; }
        public UserReadDto User { get; set; }
    }
}
