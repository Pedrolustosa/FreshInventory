using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Features.Suppliers.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Suppliers.Handlers
{
    public class CreateSupplierCommandHandler(
        ISupplierRepository supplierRepository,
        IMapper mapper,
        ILogger<CreateSupplierCommandHandler> logger) : IRequestHandler<CreateSupplierCommand, SupplierReadDto>
    {
        private readonly ISupplierRepository _supplierRepository = supplierRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateSupplierCommandHandler> _logger = logger;

        public async Task<SupplierReadDto> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            if (request?.SupplierCreateDto == null)
            {
                _logger.LogWarning("Received null data for supplier creation.");
                throw new ArgumentNullException(nameof(request), "SupplierCreateDto cannot be null.");
            }

            try
            {
                _logger.LogInformation("Creating supplier: {SupplierName}", request.SupplierCreateDto.Name);

                var supplier = new Supplier(
                    request.SupplierCreateDto.Name,
                    request.SupplierCreateDto.Address,
                    request.SupplierCreateDto.Contact,
                    request.SupplierCreateDto.Email,
                    request.SupplierCreateDto.Phone,
                    request.SupplierCreateDto.Status
                );

                var createdSupplier = await _supplierRepository.AddAsync(supplier);

                _logger.LogInformation("Supplier created successfully with ID: {SupplierId}", createdSupplier.Id);

                return _mapper.Map<SupplierReadDto>(createdSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating supplier: {SupplierName}", request.SupplierCreateDto.Name);
                throw;
            }
        }
    }
}
