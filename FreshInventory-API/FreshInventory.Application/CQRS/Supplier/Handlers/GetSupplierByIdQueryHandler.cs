using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Features.Suppliers.Queries;
using MediatR;

namespace FreshInventory.Application.Features.Suppliers.Handlers
{
    public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, SupplierReadDto>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public GetSupplierByIdQueryHandler(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<SupplierReadDto> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.Id);
            if (supplier == null)
                throw new KeyNotFoundException($"Supplier with ID {request.Id} not found.");

            return _mapper.Map<SupplierReadDto>(supplier);
        }
    }
}
