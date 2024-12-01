using FreshInventory.Application.DTO.UserDTO;
using MediatR;

namespace FreshInventory.Application.Features.Users.Queries
{
    public class GetUserByIdQuery(Guid userId) : IRequest<UserReadDto>
    {
        public Guid UserId { get; set; } = userId;
    }
}
