using MediatR;
using FreshInventory.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.DTO.UserDTO;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Users.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetUserByIdQueryHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(request.UserId);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", request.UserId);
                    throw new QueryException($"User with ID {request.UserId} not found.");
                }

                var userDto = _mapper.Map<UserDto>(user);

                _logger.LogInformation("User with ID {UserId} retrieved successfully.", request.UserId);

                return userDto;
            }
            catch (QueryException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user with ID {UserId}.", request.UserId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving user with ID {UserId}.", request.UserId);
                throw new QueryException("An unexpected error occurred while retrieving the user.", ex);
            }
        }
    }
}
