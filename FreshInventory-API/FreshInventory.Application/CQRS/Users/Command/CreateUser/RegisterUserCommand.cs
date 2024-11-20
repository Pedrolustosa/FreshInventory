using MediatR;

namespace FreshInventory.Application.CQRS.Users.Command.CreateUser
{
    public class RegisterUserCommand : IRequest<bool>
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
