using MediatR;
using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Features.Users.Queries;
using FreshInventory.Application.DTO.UserDTO;

namespace FreshInventory.Application.Features.Users.Handlers
{
    public class GetUserByEmailQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserByEmailQuery, UserReadDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<UserReadDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                throw new RepositoryException($"Usuário com email {request.Email} não encontrado.");
            }

            var userReadDto = _mapper.Map<UserReadDto>(user);
            return userReadDto;
        }
    }
}
