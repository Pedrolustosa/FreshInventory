using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Features.Suppliers.Commands;
using FreshInventory.Application.CQRS.Supplier.Commands;

namespace FreshInventory.Application.Profiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<Supplier, SupplierReadDto>().ReverseMap();
            CreateMap<SupplierCreateDto, Supplier>();
            CreateMap<SupplierUpdateDto, Supplier>();
            CreateMap<CreateSupplierCommand, Supplier>();
            CreateMap<UpdateSupplierCommand, Supplier>();
        }
    }
}
