using FreshInventory.Application.DTO.UserDTO;
using MediatR;

public class UpdateUserCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
    public required string FullName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
