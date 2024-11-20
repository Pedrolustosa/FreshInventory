using FreshInventory.Application.DTO.UserDTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Users.Command.LoginUser
{
    public class LoginUserCommand() : IRequest<LoginUserResponseDto>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}