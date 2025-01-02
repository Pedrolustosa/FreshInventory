using AutoMapper;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.CQRS.Supplier.Commands;
using FreshInventory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.CQRS.Supplier.Handlers
{
    public class UpdateSupplierCommandHandler(
        ISupplierRepository supplierRepository,
        IMapper mapper,
        ILogger<UpdateSupplierCommandHandler> logger) : IRequestHandler<UpdateSupplierCommand, SupplierReadDto>
    {
        private readonly ISupplierRepository _supplierRepository = supplierRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateSupplierCommandHandler> _logger = logger;

        public async Task<SupplierReadDto> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            if (request?.SupplierUpdateDto == null)
            {
                _logger.LogWarning("Received null data for supplier update.");
                throw new ArgumentNullException(nameof(request), "SupplierUpdateDto cannot be null.");
            }

            try
            {
                _logger.LogInformation("Updating supplier with ID {SupplierId}.", request.SupplierId);

                var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID {SupplierId} not found.", request.SupplierId);
                    throw new KeyNotFoundException($"Supplier with ID {request.SupplierId} not found.");
                }

                supplier.Update(
                    request.SupplierUpdateDto.Name,
                    request.SupplierUpdateDto.Address,
                    request.SupplierUpdateDto.Contact,
                    request.SupplierUpdateDto.Email,
                    request.SupplierUpdateDto.Phone,
                    request.SupplierUpdateDto.Category,
                    request.SupplierUpdateDto.Status
                );

                var isUpdated = await _supplierRepository.UpdateAsync(supplier);
                if (!isUpdated)
                {
                    _logger.LogWarning("Failed to update supplier with ID {SupplierId}.", request.SupplierId);
                    throw new Exception("Failed to update supplier.");
                }

                _logger.LogInformation("Supplier with ID {SupplierId} updated successfully.", request.SupplierId);

                return _mapper.Map<SupplierReadDto>(supplier);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Key not found exception while updating supplier with ID {SupplierId}.", request.SupplierId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating supplier with ID {SupplierId}.", request.SupplierId);
                throw;
            }
        }
    }
}
