using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Features.Suppliers.Commands;
using MediatR;

namespace FreshInventory.Application.Features.Suppliers.Handlers
{
    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierReadDto>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public CreateSupplierCommandHandler(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<SupplierReadDto> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = _mapper.Map<Supplier>(request.SupplierCreateDto);
            var createdSupplier = await _supplierRepository.AddAsync(supplier);
            return _mapper.Map<SupplierReadDto>(createdSupplier);
        }
    }
}
