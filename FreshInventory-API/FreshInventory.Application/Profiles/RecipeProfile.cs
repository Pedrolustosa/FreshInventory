using AutoMapper;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.Features.Recipes.Commands;
using FreshInventory.Domain.Entities;
using System.Linq;

namespace FreshInventory.Application.Profiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeReadDto>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                    src.Ingredients.Select(ri => new RecipeIngredientDto
                    {
                        IngredientId = ri.IngredientId,
                        Quantity = ri.QuantityRequired
                    })))
                .ReverseMap();

            CreateMap<RecipeCreateDto, Recipe>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                    src.Ingredients.Select(i => new RecipeIngredient(i.IngredientId, i.Quantity))));

            CreateMap<RecipeUpdateDto, Recipe>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                    src.Ingredients.Select(i => new RecipeIngredient(i.IngredientId, i.Quantity))));

            CreateMap<CreateRecipeCommand, Recipe>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                    src.RecipeCreateDto.Ingredients.Select(i => new RecipeIngredient(i.IngredientId, i.Quantity))));

            CreateMap<UpdateRecipeCommand, Recipe>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                    src.RecipeUpdateDto.Ingredients.Select(i => new RecipeIngredient(i.IngredientId, i.Quantity))));
        }
    }
}
