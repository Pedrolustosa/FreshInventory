using AutoMapper;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Application.CQRS.Ingredient.Commands;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Application.Profiles
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<Ingredient, IngredientReadDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.Quantity * src.UnitCost))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitCost, opt => opt.MapFrom(src => src.UnitCost))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate));

            CreateMap<IngredientCreateDto, CreateIngredientCommand>()
                .ConstructUsing(src => new CreateIngredientCommand(
                    src.Name,
                    src.Quantity,
                    src.UnitCost,
                    src.SupplierId));

            CreateMap<CreateIngredientCommand, Ingredient>()
                .ConstructUsing(src => new Ingredient(
                    src.Name,
                    src.Quantity,
                    src.UnitCost,
                    null))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId));

            CreateMap<UpdateIngredientCommand, Ingredient>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IngredientId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.IngredientUpdateDto.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.IngredientUpdateDto.Quantity))
                .ForMember(dest => dest.UnitCost, opt => opt.MapFrom(src => src.IngredientUpdateDto.UnitCost))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.IngredientUpdateDto.SupplierId))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

            CreateMap<IngredientUpdateDto, Ingredient>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitCost, opt => opt.MapFrom(src => src.UnitCost))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
        }
    }
}