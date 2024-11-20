using AutoMapper;
using FreshInventory.Application.CQRS.Suppliers.Command.CreateSupplier;
using FreshInventory.Application.CQRS.Suppliers.Command.UpdateSupplier;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Application.Mappings
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<SupplierCreateDto, CreateSupplierCommand>();
            CreateMap<SupplierUpdateDto, UpdateSupplierCommand>();

            CreateMap<CreateSupplierCommand, Supplier>();
            CreateMap<UpdateSupplierCommand, Supplier>();

            CreateMap<Supplier, SupplierDto>();
        }
    }
}
