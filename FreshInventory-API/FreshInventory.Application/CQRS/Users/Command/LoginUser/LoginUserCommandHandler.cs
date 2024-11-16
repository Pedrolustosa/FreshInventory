using MediatR;
using FreshInventory.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FreshInventory.Application.DTO.UserDTO;
using FreshInventory.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.CQRS.Users.Command.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginUserCommandHandler> _logger;

        public LoginUserCommandHandler(IUserRepository userRepository, IConfiguration configuration, ILogger<LoginUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<LoginUserResponseDto?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(request.Email);

                if (user == null || !await _userRepository.CheckPasswordAsync(user, request.Password))
                {
                    _logger.LogWarning("Invalid login attempt for email: {Email}", request.Email);
                    return null;
                }

                var token = GenerateJwtToken(user);
                _logger.LogInformation("User {Email} logged in successfully.", user.Email);

                return new LoginUserResponseDto(token, user.FullName, user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in user {Email}.", request.Email);
                throw new Exception("An unexpected error occurred during login.", ex);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = _configuration["JwtSettings:SecurityKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            if (string.IsNullOrEmpty(securityKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                _logger.LogError("JWT configuration is missing or invalid.");
                throw new Exception("JWT configuration is missing or invalid.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName", user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
