using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.Mappings;

public class RecipeIngredientProfile : Profile
{
    public RecipeIngredientProfile()
    {
        CreateMap<RecipeIngredient, RecipeIngredientDto>()
            .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient.Name));

        CreateMap<RecipeIngredientCreateDto, RecipeIngredient>();
    }
}
