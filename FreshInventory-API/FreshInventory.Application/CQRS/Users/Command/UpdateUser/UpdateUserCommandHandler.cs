using MediatR;
using AutoMapper;
using FreshInventory.Domain.Interfaces;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.Id);

        if (user == null)
        {
            return false;
        }

        _mapper.Map(request, user);
        return await _userRepository.UpdateUserAsync(user);
    }
}
