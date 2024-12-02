using MediatR;
using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FluentValidation;
using FreshInventory.Application.Features.Users.Commands;
using FreshInventory.Application.DTO.UserDTO;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Users.Handlers;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IValidator<UpdateUserCommand> validator,
    ILogger<UpdateUserCommandHandler> logger) : IRequestHandler<UpdateUserCommand, UserReadDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<UpdateUserCommand> _validator = validator;
    private readonly ILogger<UpdateUserCommandHandler> _logger = logger;

    public async Task<UserReadDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing user update for User ID: {UserId}", request.UserId);

        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for updating user with ID: {UserId}", request.UserId);
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for update.", request.UserId);
                throw new RepositoryException($"User with ID {request.UserId} not found.");
            }

            _mapper.Map(request, user);

            var isUpdated = await _userRepository.UpdateUserAsync(user);
            if (!isUpdated)
            {
                _logger.LogWarning("Failed to update user with ID {UserId}.", request.UserId);
                throw new RepositoryException("Failed to update the user.");
            }

            var userReadDto = _mapper.Map<UserReadDto>(user);
            _logger.LogInformation("User with ID {UserId} updated successfully.", request.UserId);

            return userReadDto;
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation exception during user update for User ID: {UserId}", request.UserId);
            throw;
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception during user update for User ID: {UserId}", request.UserId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while updating user with ID: {UserId}", request.UserId);
            throw;
        }
    }
}
