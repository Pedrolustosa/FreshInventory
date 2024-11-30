using MediatR;
using FreshInventory.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.DTO.UserDTO;
using FreshInventory.Application.Exceptions;
using FreshInventory.Application.CQRS.Users.Queries.GetUserByEmail;

namespace FreshInventory.Application.CQRS.Users.Queries
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserByEmailQueryHandler> _logger;

        public GetUserByEmailQueryHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetUserByEmailQueryHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(request.Email);

                if (user == null)
                {
                    _logger.LogWarning("User with email {Email} not found.", request.Email);
                    throw new QueryException($"User with email {request.Email} not found.");
                }

                var userDto = _mapper.Map<UserDto>(user);

                _logger.LogInformation("User with email {Email} retrieved successfully.", request.Email);

                return userDto;
            }
            catch (QueryException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user with email {Email}.", request.Email);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving user with email {Email}.", request.Email);
                throw new QueryException("An unexpected error occurred while retrieving the user.", ex);
            }
        }
    }
}
