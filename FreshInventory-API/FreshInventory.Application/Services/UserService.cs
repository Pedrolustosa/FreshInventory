using AutoMapper;
using FreshInventory.Application.DTO.UserDTO;
using FreshInventory.Application.Features.Users.Commands;
using FreshInventory.Application.Features.Users.Queries;
using FreshInventory.Application.Interfaces;
using FreshInventory.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Services;

public class UserService(IMediator mediator, IMapper mapper, ILogger<UserService> logger) : IUserService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<UserReadDto> RegisterUserAsync(UserCreateDto registerUserDto)
    {
        if (registerUserDto == null)
        {
            _logger.LogWarning("Received null data for user registration.");
            throw new ArgumentNullException(nameof(registerUserDto), "RegisterUserDto cannot be null.");
        }

        try
        {
            var command = _mapper.Map<CreateUserCommand>(registerUserDto);
            var userReadDto = await _mediator.Send(command);

            if (userReadDto != null)
            {
                _logger.LogInformation("User registered successfully: {Email}", registerUserDto.Email);
                return userReadDto;
            }

            _logger.LogWarning("User registration failed for email: {Email}", registerUserDto.Email);
            throw new RepositoryException("User registration failed.");
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception during user registration for email: {Email}", registerUserDto.Email);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user registration for email: {Email}", registerUserDto.Email);
            throw;
        }
    }

    public async Task<UserLoginResponseDto> LoginUserAsync(UserLoginDto loginUserDto)
    {
        if (loginUserDto == null)
        {
            _logger.LogWarning("Received null data for user login.");
            throw new ArgumentNullException(nameof(loginUserDto), "LoginUserDto cannot be null.");
        }

        try
        {
            var command = _mapper.Map<LoginUserCommand>(loginUserDto);
            var response = await _mediator.Send(command);

            if (response != null)
            {
                _logger.LogInformation("User logged in successfully: {Email}", loginUserDto.Email);
                return response;
            }

            _logger.LogWarning("Invalid login credentials for email: {Email}", loginUserDto.Email);
            throw new UnauthorizedAccessException("Invalid credentials.");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access attempt for email: {Email}", loginUserDto.Email);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user login for email: {Email}", loginUserDto.Email);
            throw;
        }
    }

    public async Task<UserReadDto> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.LogWarning("Received null or empty email for GetUserByEmailAsync.");
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }

        try
        {
            var query = new GetUserByEmailQuery(email);
            var userReadDto = await _mediator.Send(query);

            if (userReadDto != null)
            {
                _logger.LogInformation("User with email {Email} retrieved successfully.", email);
                return userReadDto;
            }

            _logger.LogWarning("User with email {Email} not found.", email);
            throw new RepositoryException($"User with email {email} not found.");
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "User with email {Email} not found.", email);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving user with email {Email}.", email);
            throw;
        }
    }

    public async Task<UserReadDto> GetUserByIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            _logger.LogWarning("Received null or empty userId for GetUserByIdAsync.");
            throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
        }

        try
        {
            var query = new GetUserByIdQuery(userId);
            var userReadDto = await _mediator.Send(query);

            if (userReadDto != null)
            {
                _logger.LogInformation("User with ID {UserId} retrieved successfully.", userId);
                return userReadDto;
            }

            _logger.LogWarning("User with ID {UserId} not found.", userId);
            throw new RepositoryException($"User with ID {userId} not found.");
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "User with ID {UserId} not found.", userId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving user with ID {UserId}.", userId);
            throw;
        }
    }

    public async Task<UserReadDto> UpdateUserAsync(Guid userId, UserUpdateDto updateUserDto)
    {
        if (updateUserDto == null)
        {
            _logger.LogWarning("Received null data for user update.");
            throw new ArgumentNullException(nameof(updateUserDto), "UpdateUserDto cannot be null.");
        }

        if (userId == Guid.Empty)
        {
            _logger.LogWarning("Received null or empty userId for UpdateUserAsync.");
            throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
        }

        try
        {
            updateUserDto.UserId = userId;
            var command = _mapper.Map<UpdateUserCommand>(updateUserDto);
            var updatedUserReadDto = await _mediator.Send(command);

            if (updatedUserReadDto != null)
            {
                _logger.LogInformation("User with ID {UserId} updated successfully.", userId);
                return updatedUserReadDto;
            }

            _logger.LogWarning("Failed to update user with ID {UserId}.", userId);
            throw new RepositoryException($"Failed to update user with ID {userId}.");
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Failed to update user with ID {UserId}.", userId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating user with ID {UserId}.", userId);
            throw;
        }
    }
}
