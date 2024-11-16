using MediatR;
using FreshInventory.Application.DTO.UserDTO;

namespace FreshInventory.Application.CQRS.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public string UserId { get; set; }
    }
}
