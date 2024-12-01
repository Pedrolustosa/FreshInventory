using AutoMapper;
using FreshInventory.Application.DTO.UserDTO;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.Features.Users.Commands;

namespace FreshInventory.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserLoginDto, LoginUserCommand>().ReverseMap();

            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserReadDto>();
            CreateMap<UserUpdateDto, User>();

            CreateMap<CreateUserCommand, User>();
            CreateMap<UserCreateDto, CreateUserCommand>();

            CreateMap<UpdateUserCommand, User>();
            CreateMap<UserUpdateDto, UpdateUserCommand>();
        }
    }
}
