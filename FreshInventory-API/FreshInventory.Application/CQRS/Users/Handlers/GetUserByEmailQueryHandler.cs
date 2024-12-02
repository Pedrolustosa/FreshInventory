using MediatR;
using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Features.Users.Queries;
using FreshInventory.Application.DTO.UserDTO;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Users.Handlers;

public class GetUserByEmailQueryHandler(
    IUserRepository userRepository,
    IMapper mapper,
    ILogger<GetUserByEmailQueryHandler> logger) : IRequestHandler<GetUserByEmailQuery, UserReadDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetUserByEmailQueryHandler> _logger = logger;

    public async Task<UserReadDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetUserByEmailQuery for email: {Email}", request.Email);

        try
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found.", request.Email);
                throw new RepositoryException($"User with email {request.Email} not found.");
            }

            var userReadDto = _mapper.Map<UserReadDto>(user);
            _logger.LogInformation("User with email {Email} retrieved successfully.", request.Email);

            return userReadDto;
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception while retrieving user with email: {Email}", request.Email);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling GetUserByEmailQuery for email: {Email}", request.Email);
            throw;
        }
    }
}
