using MediatR;
using FreshInventory.Domain.Interfaces;
using AutoMapper;
using FreshInventory.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.CQRS.Users.Command.CreateUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _mapper.Map<User>(request);
                var result = await _userRepository.RegisterUserAsync(user, request.Password);

                if (result)
                {
                    _logger.LogInformation("User '{Email}' registered successfully.", request.Email);
                }
                else
                {
                    _logger.LogWarning("Failed to register user '{Email}'.", request.Email);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while registering user '{Email}'.", request.Email);
                throw new Exception("An unexpected error occurred while registering the user.", ex);
            }
        }
    }
}
