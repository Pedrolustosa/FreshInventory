using AutoMapper;
using FreshInventory.Application.CQRS.Users.Command.CreateUser;
using FreshInventory.Application.CQRS.Users.Command.LoginUser;
using FreshInventory.Application.DTO.UserDTO;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RegisterUserDto, RegisterUserCommand>().ReverseMap();
            CreateMap<RegisterUserCommand, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<LoginUserDto, LoginUserCommand>().ReverseMap();
            CreateMap<UpdateUserDto, User>().ReverseMap();
        }
    }
}
