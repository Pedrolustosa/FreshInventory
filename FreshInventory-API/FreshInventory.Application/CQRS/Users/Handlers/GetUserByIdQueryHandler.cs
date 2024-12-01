using MediatR;
using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Features.Users.Queries;
using FreshInventory.Application.DTO.UserDTO;

namespace FreshInventory.Application.Features.Users.Handlers
{
    public class GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserReadDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<UserReadDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new RepositoryException($"Usuário com ID {request.UserId} não encontrado.");
            }

            var userReadDto = _mapper.Map<UserReadDto>(user);
            return userReadDto;
        }
    }
}
