using MediatR;
using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FluentValidation;
using FreshInventory.Application.Features.Users.Commands;
using FreshInventory.Application.DTO.UserDTO;

namespace FreshInventory.Application.Features.Users.Handlers
{
    public class CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IValidator<CreateUserCommand> validator) : IRequestHandler<CreateUserCommand, UserReadDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IValidator<CreateUserCommand> _validator = validator;

        public async Task<UserReadDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = _mapper.Map<User>(request);

            var isRegistered = await _userRepository.RegisterUserAsync(user, request.Password);
            if (!isRegistered)
            {
                throw new RepositoryException("Falha ao registrar o usuário.");
            }

            var userReadDto = _mapper.Map<UserReadDto>(user);
            return userReadDto;
        }
    }
}
