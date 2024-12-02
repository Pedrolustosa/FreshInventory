using AutoMapper;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.Features.Ingredients.Commands;
using FreshInventory.Application.CQRS.Ingredient.Commands;

namespace FreshInventory.Application.Profiles
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            // Entity to DTO
            CreateMap<Ingredient, IngredientReadDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));

            // DTO to Command
            CreateMap<IngredientCreateDto, Ingredient>();
            CreateMap<IngredientUpdateDto, Ingredient>();

            // Command to Entity
            CreateMap<CreateIngredientCommand, Ingredient>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.IngredientCreateDto.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.IngredientCreateDto.Quantity))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.IngredientCreateDto.Unit))
                .ForMember(dest => dest.UnitCost, opt => opt.MapFrom(src => src.IngredientCreateDto.UnitCost))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.IngredientCreateDto.Category))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.IngredientCreateDto.SupplierId))
                .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.IngredientCreateDto.PurchaseDate))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.IngredientCreateDto.ExpiryDate))
                .ForMember(dest => dest.IsPerishable, opt => opt.MapFrom(src => src.IngredientCreateDto.IsPerishable))
                .ForMember(dest => dest.ReorderLevel, opt => opt.MapFrom(src => src.IngredientCreateDto.ReorderLevel));

            CreateMap<UpdateIngredientCommand, Ingredient>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IngredientId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.IngredientUpdateDto.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.IngredientUpdateDto.Quantity))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.IngredientUpdateDto.Unit))
                .ForMember(dest => dest.UnitCost, opt => opt.MapFrom(src => src.IngredientUpdateDto.UnitCost))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.IngredientUpdateDto.Category))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.IngredientUpdateDto.SupplierId))
                .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.IngredientUpdateDto.PurchaseDate))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.IngredientUpdateDto.ExpiryDate))
                .ForMember(dest => dest.IsPerishable, opt => opt.MapFrom(src => src.IngredientUpdateDto.IsPerishable))
                .ForMember(dest => dest.ReorderLevel, opt => opt.MapFrom(src => src.IngredientUpdateDto.ReorderLevel));
        }
    }
}