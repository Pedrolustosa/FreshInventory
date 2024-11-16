using FreshInventory.Application.DTO.UserDTO;
using MediatR;

public class UpdateUserCommand : IRequest<bool>
{
    public UpdateUserDto UpdateUserDto { get; set; }

    public UpdateUserCommand(UpdateUserDto updateUserDto)
    {
        UpdateUserDto = updateUserDto;
    }
}
