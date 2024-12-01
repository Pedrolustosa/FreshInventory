using MediatR;
using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FluentValidation;
using FreshInventory.Application.Features.Users.Commands;
using FreshInventory.Application.DTO.UserDTO;

namespace FreshInventory.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IValidator<UpdateUserCommand> validator) : IRequestHandler<UpdateUserCommand, UserReadDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IValidator<UpdateUserCommand> _validator = validator;

        public async Task<UserReadDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new RepositoryException($"Usuário com ID {request.UserId} não encontrado.");
            }

            _mapper.Map(request, user);

            var isUpdated = await _userRepository.UpdateUserAsync(user);
            if (!isUpdated)
            {
                throw new RepositoryException("Falha ao atualizar o usuário.");
            }

            var userReadDto = _mapper.Map<UserReadDto>(user);
            return userReadDto;
        }
    }
}
