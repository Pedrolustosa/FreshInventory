using AutoMapper;
using FreshInventory.Application.CQRS.Ingredients.Commands.CreateIngredient;
using FreshInventory.Application.CQRS.Ingredients.Commands.UpdateIngredient;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Application.Mappings
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<Ingredient, IngredientDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));

            CreateMap<IngredientCreateDto, Ingredient>();
            CreateMap<IngredientUpdateDto, Ingredient>();

            CreateMap<IngredientCreateDto, CreateIngredientCommand>();
            CreateMap<IngredientUpdateDto, UpdateIngredientCommand>();
        }
    }
}