using MediatR;
using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FluentValidation;
using FreshInventory.Application.Features.Users.Commands;
using FreshInventory.Application.DTO.UserDTO;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Users.Handlers;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IValidator<CreateUserCommand> validator,
    ILogger<CreateUserCommandHandler> logger) : IRequestHandler<CreateUserCommand, UserReadDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<CreateUserCommand> _validator = validator;
    private readonly ILogger<CreateUserCommandHandler> _logger = logger;

    public async Task<UserReadDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting CreateUserCommand handling.");

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for CreateUserCommand. Errors: {Errors}",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        var user = _mapper.Map<User>(request);

        try
        {
            var isRegistered = await _userRepository.RegisterUserAsync(user, request.Password);
            if (!isRegistered)
            {
                _logger.LogWarning("Failed to register user: {UserName}", user.UserName);
                throw new RepositoryException("Failed to register the user.");
            }

            var userReadDto = _mapper.Map<UserReadDto>(user);
            _logger.LogInformation("User successfully registered with email: {Email}", user.Email);

            return userReadDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling CreateUserCommand for email: {Email}", user.Email);
            throw;
        }
    }
}
