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
            CreateMap<Recipe, RecipeReadDto>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                    src.Ingredients.Select(i => new RecipeIngredientDto
                    {
                        IngredientId = i.Key,
                        Quantity = i.Value
                    })));

            CreateMap<RecipeCreateDto, Recipe>()
                .ConstructUsing(src => new Recipe(
                    src.Name,
                    src.Description,
                    src.Servings,
                    src.PreparationTime,
                    src.Ingredients != null
                        ? src.Ingredients.ToDictionary(i => i.IngredientId, i => i.Quantity)
                        : new Dictionary<int, int>(),
                    src.Steps ?? new List<string>()
                ))
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore());

            CreateMap<RecipeUpdateDto, Recipe>()
                .ConstructUsing(src => new Recipe(
                    src.Name,
                    src.Description,
                    src.Servings,
                    src.PreparationTime,
                    src.Ingredients != null
                        ? src.Ingredients.ToDictionary(i => i.IngredientId, i => i.Quantity)
                        : new Dictionary<int, int>(),
                    src.Steps ?? new List<string>()
                ))
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore());

            CreateMap<CreateRecipeCommand, Recipe>()
                .ConstructUsing(src => new Recipe(
                    src.RecipeCreateDto.Name,
                    src.RecipeCreateDto.Description,
                    src.RecipeCreateDto.Servings,
                    src.RecipeCreateDto.PreparationTime,
                    src.RecipeCreateDto.Ingredients != null
                        ? src.RecipeCreateDto.Ingredients.ToDictionary(i => i.IngredientId, i => i.Quantity)
                        : new Dictionary<int, int>(),
                    src.RecipeCreateDto.Steps ?? new List<string>()
                ))
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore());

            CreateMap<UpdateRecipeCommand, Recipe>()
                .ConstructUsing(src => new Recipe(
                    src.RecipeUpdateDto.Name,
                    src.RecipeUpdateDto.Description,
                    src.RecipeUpdateDto.Servings,
                    src.RecipeUpdateDto.PreparationTime,
                    src.RecipeUpdateDto.Ingredients != null
                        ? src.RecipeUpdateDto.Ingredients.ToDictionary(i => i.IngredientId, i => i.Quantity)
                        : new Dictionary<int, int>(),
                    src.RecipeUpdateDto.Steps ?? new List<string>()
                ))
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore());
        }
    }
}
