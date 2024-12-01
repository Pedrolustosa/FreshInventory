using MediatR;
using FreshInventory.Application.DTO.UserDTO;

namespace FreshInventory.Application.Features.Users.Commands
{
    public class LoginUserCommand : IRequest<UserLoginResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
