using MediatR;

namespace FreshInventory.Application.CQRS.Users.Command.CreateUser
{
    public class RegisterUserCommand : IRequest<bool>
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
