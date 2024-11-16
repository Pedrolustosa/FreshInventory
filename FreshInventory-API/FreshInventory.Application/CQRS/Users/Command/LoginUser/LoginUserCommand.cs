using FreshInventory.Application.DTO;
using FreshInventory.Application.DTO.UserDTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Users.Command.LoginUser
{
    public class LoginUserCommand(string email, string password) : IRequest<LoginUserResponseDto>
    {
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;
    }
}