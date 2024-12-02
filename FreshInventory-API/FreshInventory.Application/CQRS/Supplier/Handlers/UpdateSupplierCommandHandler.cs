using MediatR;
using AutoMapper;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.CQRS.Supplier.Commands;
using FreshInventory.Domain.Interfaces;

namespace FreshInventory.Application.CQRS.Supplier.Handlers
{
    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, SupplierReadDto>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public UpdateSupplierCommandHandler(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<SupplierReadDto> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId);
            if (supplier == null)
            {
                throw new KeyNotFoundException($"Supplier with ID {request.SupplierId} not found.");
            }

            // Atualizar os dados do fornecedor
            _mapper.Map(request.SupplierUpdateDto, supplier);

            // Salvar as alterações
            var isUpdated = await _supplierRepository.UpdateAsync(supplier);
            if (!isUpdated)
            {
                throw new Exception("Failed to update supplier.");
            }

            // Retornar o DTO atualizado
            return _mapper.Map<SupplierReadDto>(supplier);
        }
    }
}
