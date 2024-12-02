using MediatR;
using AutoMapper;
using FreshInventory.Application.DTO.UserDTO;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using FreshInventory.Application.Features.Users.Commands;

namespace FreshInventory.Application.Features.Users.Handlers;

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IConfiguration configuration,
    IValidator<LoginUserCommand> validator,
    ILogger<LoginUserCommandHandler> logger) : IRequestHandler<LoginUserCommand, UserLoginResponseDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IConfiguration _configuration = configuration;
    private readonly IValidator<LoginUserCommand> _validator = validator;
    private readonly ILogger<LoginUserCommandHandler> _logger = logger;

    public async Task<UserLoginResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing login for email: {Email}", request.Email);

        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for login attempt with email: {Email}", request.Email);
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found.", request.Email);
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var isPasswordValid = await _userRepository.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Invalid password for email: {Email}", request.Email);
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = GenerateJwtToken(user);

            var userReadDto = _mapper.Map<UserReadDto>(user);

            _logger.LogInformation("User logged in successfully: {Email}", request.Email);

            return new UserLoginResponseDto
            {
                Token = token,
                User = userReadDto
            };
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized login attempt for email: {Email}", request.Email);
            throw;
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation exception during login for email: {Email}", request.Email);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during login for email: {Email}", request.Email);
            throw;
        }
    }

    private string GenerateJwtToken(User user)
    {
        try
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expirationMinutesString = jwtSettings["ExpiresInMinutes"];

            if (string.IsNullOrWhiteSpace(secretKey) ||
                string.IsNullOrWhiteSpace(issuer) ||
                string.IsNullOrWhiteSpace(audience) ||
                string.IsNullOrWhiteSpace(expirationMinutesString))
            {
                _logger.LogError("JWT configuration is missing or invalid.");
                throw new InvalidOperationException("JWT configuration is missing or invalid.");
            }

            if (!int.TryParse(expirationMinutesString, out var expirationMinutes))
            {
                _logger.LogError("'ExpiresInMinutes' in JWT settings is not a valid integer.");
                throw new InvalidOperationException("'ExpiresInMinutes' must be a valid integer.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("fullName", user.FullName)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while generating JWT token for user: {UserId}", user.Id);
            throw new InvalidOperationException("An error occurred while generating the JWT token.", ex);
        }
    }
}
