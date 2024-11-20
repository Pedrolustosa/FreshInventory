using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.DTO.RecipeDTO;

namespace FreshInventory.Application.Mappings;

public class RecipeIngredientProfile : Profile
{
    public RecipeIngredientProfile()
    {
        CreateMap<RecipeIngredient, RecipeIngredientDto>()
            .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.IngredientId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

        CreateMap<RecipeIngredientCreateDto, RecipeIngredient>();
    }
}