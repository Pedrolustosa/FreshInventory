using MediatR;
using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Features.Users.Queries;
using FreshInventory.Application.DTO.UserDTO;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Users.Handlers;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    IMapper mapper,
    ILogger<GetUserByIdQueryHandler> logger) : IRequestHandler<GetUserByIdQuery, UserReadDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetUserByIdQueryHandler> _logger = logger;

    public async Task<UserReadDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetUserByIdQuery for ID: {UserId}", request.UserId);

        try
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", request.UserId);
                throw new RepositoryException($"User with ID {request.UserId} not found.");
            }

            var userReadDto = _mapper.Map<UserReadDto>(user);
            _logger.LogInformation("User with ID {UserId} retrieved successfully.", request.UserId);

            return userReadDto;
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception while retrieving user with ID: {UserId}", request.UserId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling GetUserByIdQuery for ID: {UserId}", request.UserId);
            throw;
        }
    }
}
