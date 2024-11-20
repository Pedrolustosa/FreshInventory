using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.CQRS.Commands.CreateRecipe;
using FreshInventory.Application.CQRS.Commands.UpdateRecipe;
using FreshInventory.Application.CQRS.Recipes.Command.CreateRecipe;

namespace FreshInventory.Application.Mappings;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        // Domain <-> DTO
        CreateMap<Recipe, RecipeDto>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ReverseMap();

        CreateMap<RecipeIngredient, RecipeIngredientDto>()
            .ReverseMap();

        // DTO <-> Domain
        CreateMap<RecipeCreateDto, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ReverseMap();

        CreateMap<RecipeIngredientCreateDto, RecipeIngredient>()
            .ReverseMap();

        CreateMap<RecipeUpdateDto, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ReverseMap();

        // CQRS Commands -> Domain
        CreateMap<CreateRecipeCommand, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ReverseMap();

        CreateMap<CreateRecipeIngredientCommand, RecipeIngredient>()
            .ReverseMap();

        // DTO -> Commands
        CreateMap<RecipeCreateDto, CreateRecipeCommand>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));

        CreateMap<RecipeIngredientCreateDto, CreateRecipeIngredientCommand>()
            .ReverseMap();

        // Update Commands -> Domain
        CreateMap<UpdateRecipeCommand, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ReverseMap();
    }
}
