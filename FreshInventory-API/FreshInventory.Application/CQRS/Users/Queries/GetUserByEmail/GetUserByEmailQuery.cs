using FreshInventory.Application.DTO.UserDTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Users.Queries.GetUserByEmail
{
    public class GetUserByEmailQuery : IRequest<UserDto>
    {
        public string Email { get; set; }
    }
}
