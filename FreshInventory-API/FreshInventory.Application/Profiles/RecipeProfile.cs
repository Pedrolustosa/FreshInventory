using AutoMapper;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.Features.Recipes.Commands;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Application.Profiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            // Mapeando Recipe para RecipeReadDto
            CreateMap<Recipe, RecipeReadDto>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                    src.RecipeIngredients.Select(ri => new RecipeIngredientDto
                    {
                        IngredientId = ri.IngredientId,
                        IngredientName = ri.Ingredient.Name,
                        Quantity = ri.Quantity
                    }).ToList()));

            // Mapeando RecipeCreateDto para Recipe
            CreateMap<RecipeCreateDto, Recipe>()
                .ConstructUsing(src => new Recipe(
                    src.Name,
                    src.Description,
                    src.Servings,
                    src.PreparationTime,
                    src.Ingredients.Select(i => new RecipeIngredient
                    {
                        IngredientId = i.IngredientId,
                        Quantity = i.Quantity
                    }).ToList(),
                    src.Steps
                ));

            // Mapeando RecipeUpdateDto para Recipe
            CreateMap<RecipeUpdateDto, Recipe>()
                .ForMember(dest => dest.RecipeIngredients, opt => opt.MapFrom(src =>
                    src.Ingredients.Select(i => new RecipeIngredient
                    {
                        IngredientId = i.IngredientId,
                        Quantity = i.Quantity
                    }).ToList()));

            // Map CreateRecipeCommand to Recipe
            CreateMap<CreateRecipeCommand, Recipe>()
                .ConstructUsing(src => new Recipe(
                    src.RecipeCreateDto.Name,
                    src.RecipeCreateDto.Description,
                    src.RecipeCreateDto.Servings,
                    src.RecipeCreateDto.PreparationTime,
                    src.RecipeCreateDto.Ingredients != null
                        ? src.RecipeCreateDto.Ingredients.Select(i => new RecipeIngredient
                        {
                            IngredientId = i.IngredientId,
                            Quantity = i.Quantity
                        }).ToList()
                        : new List<RecipeIngredient>(),
                    src.RecipeCreateDto.Steps ?? new List<string>()
                ))
                .ForMember(dest => dest.RecipeIngredients, opt => opt.Ignore());

            // Map UpdateRecipeCommand to Recipe
            CreateMap<UpdateRecipeCommand, Recipe>()
                .ConstructUsing(src => new Recipe(
                    src.RecipeUpdateDto.Name,
                    src.RecipeUpdateDto.Description,
                    src.RecipeUpdateDto.Servings,
                    src.RecipeUpdateDto.PreparationTime,
                    src.RecipeUpdateDto.Ingredients != null
                        ? src.RecipeUpdateDto.Ingredients.Select(i => new RecipeIngredient
                        {
                            IngredientId = i.IngredientId,
                            Quantity = i.Quantity
                        }).ToList()
                        : new List<RecipeIngredient>(),
                    src.RecipeUpdateDto.Steps ?? new List<string>()
                ))
                .ForMember(dest => dest.RecipeIngredients, opt => opt.Ignore());
        }
    }
}
