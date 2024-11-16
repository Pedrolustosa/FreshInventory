using AutoMapper;
using FreshInventory.Application.DTO.UserDTO;
using FreshInventory.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.CQRS.Users.Command.CreateUser;
using FreshInventory.Application.CQRS.Users.Command.LoginUser;
using FreshInventory.Application.CQRS.Users.Queries;

namespace FreshInventory.Application.Services;

public class UserService(IMediator mediator, IMapper mapper, ILogger<UserService> logger) : IUserService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        if (registerUserDto == null)
        {
            _logger.LogWarning("Received null data for user registration.");
            throw new ArgumentNullException(nameof(registerUserDto), "RegisterUserDto cannot be null.");
        }

        try
        {
            var command = _mapper.Map<RegisterUserCommand>(registerUserDto);
            var result = await _mediator.Send(command);

            if (result)
            {
                _logger.LogInformation("User registered successfully: {Email}", registerUserDto.Email);
            }
            else
            {
                _logger.LogWarning("User registration failed for email: {Email}", registerUserDto.Email);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user registration for email: {Email}", registerUserDto.Email);
            throw;
        }
    }

    public async Task<LoginUserResponseDto> LoginUserAsync(LoginUserDto loginUserDto)
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
            }
            else
            {
                _logger.LogWarning("Invalid login credentials for email: {Email}", loginUserDto.Email);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user login for email: {Email}", loginUserDto.Email);
            throw;
        }
    }

    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            _logger.LogWarning("Received null or empty userId for GetUserByIdAsync.");
            throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
        }

        try
        {
            var query = new GetUserByIdQuery { UserId = userId };
            var user = await _mediator.Send(query);

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", userId);
                return null;
            }

            _logger.LogInformation("User with ID {UserId} retrieved successfully.", userId);
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving user with ID {UserId}.", userId);
            throw;
        }
    }

    public async Task<bool> UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        var command = _mapper.Map<UpdateUserCommand>(updateUserDto);
        return await _mediator.Send(command);
    }

}
