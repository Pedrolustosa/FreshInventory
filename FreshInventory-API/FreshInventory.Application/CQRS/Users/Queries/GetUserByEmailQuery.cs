using FreshInventory.Application.DTO.UserDTO;
using MediatR;

namespace FreshInventory.Application.Features.Users.Queries
{
    public class GetUserByEmailQuery(string email) : IRequest<UserReadDto>
    {
        public string Email { get; set; } = email;
    }
}
