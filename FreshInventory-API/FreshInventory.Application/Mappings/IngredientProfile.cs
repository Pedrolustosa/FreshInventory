using AutoMapper;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Application.Mappings
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<Ingredient, IngredientDto>();
            CreateMap<IngredientCreateDto, Ingredient>();
            CreateMap<IngredientUpdateDto, Ingredient>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

            CreateMap<IngredientDto, Ingredient>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
        }
    }
}