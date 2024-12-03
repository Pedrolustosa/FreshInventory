using MediatR;
using AutoMapper;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.CQRS.Supplier.Commands;
using FreshInventory.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.CQRS.Supplier.Handlers;

public class UpdateSupplierCommandHandler(ISupplierRepository supplierRepository, IMapper mapper, ILogger<UpdateSupplierCommandHandler> logger) : IRequestHandler<UpdateSupplierCommand, SupplierReadDto>
{
    private readonly ISupplierRepository _supplierRepository = supplierRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UpdateSupplierCommandHandler> _logger = logger;

    public async Task<SupplierReadDto> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Attempting to update supplier with ID {SupplierId}.", request.SupplierId);

            var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {SupplierId} not found.", request.SupplierId);
                throw new KeyNotFoundException($"Supplier with ID {request.SupplierId} not found.");
            }

            _mapper.Map(request.SupplierUpdateDto, supplier);

            var isUpdated = await _supplierRepository.UpdateAsync(supplier);
            if (!isUpdated)
            {
                _logger.LogWarning("Failed to update supplier with ID {SupplierId}.", request.SupplierId);
                throw new Exception("Failed to update supplier.");
            }

            var updatedSupplierDto = _mapper.Map<SupplierReadDto>(supplier);
            _logger.LogInformation("Supplier with ID {SupplierId} updated successfully.", request.SupplierId);
            return updatedSupplierDto;
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
