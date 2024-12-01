using MediatR;
using AutoMapper;
using FreshInventory.Application.DTO.UserDTO;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using FreshInventory.Application.Features.Users.Commands;

namespace FreshInventory.Application.Features.Users.Handlers
{
    public class LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, IValidator<LoginUserCommand> validator) : IRequestHandler<LoginUserCommand, UserLoginResponseDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IConfiguration _configuration = configuration;
        private readonly IValidator<LoginUserCommand> _validator = validator;

        public async Task<UserLoginResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            var isPasswordValid = await _userRepository.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            var token = GenerateJwtToken(user);

            var userReadDto = _mapper.Map<UserReadDto>(user);

            var response = new UserLoginResponseDto
            {
                Token = token,
                User = userReadDto
            };

            return response;
        }

        private string GenerateJwtToken(User user)
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
                throw new InvalidOperationException("As configurações do JWT estão ausentes ou inválidas.");
            }

            if (!int.TryParse(expirationMinutesString, out var expirationMinutes))
            {
                throw new InvalidOperationException("O valor de 'ExpiresInMinutes' deve ser um número válido.");
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

    }
}
