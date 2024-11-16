using AutoMapper;
using FreshInventory.Domain.Entities;
using System.Linq;
using FreshInventory.Application.DTO.RecipeDTO;

namespace FreshInventory.Application.Mappings
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));

            CreateMap<RecipeCreateDto, Recipe>()
                .ConstructUsing(src => new Recipe(
                    src.Name,
                    src.Ingredients.Select(i => new RecipeIngredient(i.IngredientId, i.QuantityRequired)).ToList()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore()); // Ignora Ingredients no mapeamento padrão para usar o construtor

            CreateMap<RecipeUpdateDto, Recipe>()
                .ConstructUsing(src => new Recipe(
                    src.Name,
                    src.Ingredients.Select(i => new RecipeIngredient(i.IngredientId, i.QuantityRequired)).ToList()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore()); // Ignora Ingredients no mapeamento padrão para usar o construtor
        }
    }
}
