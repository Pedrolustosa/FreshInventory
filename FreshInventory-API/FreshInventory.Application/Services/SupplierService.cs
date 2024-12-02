using AutoMapper;
using FreshInventory.Application.CQRS.Supplier.Commands;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Features.Suppliers.Commands;
using FreshInventory.Application.Features.Suppliers.Queries;
using FreshInventory.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Services
{
    public class SupplierService(IMediator mediator, IMapper mapper, ILogger<SupplierService> logger) : ISupplierService
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<SupplierService> _logger = logger;

        public async Task<SupplierReadDto> CreateSupplierAsync(SupplierCreateDto supplierCreateDto)
        {
            if (supplierCreateDto == null)
            {
                _logger.LogWarning("Received null data for supplier creation.");
                throw new ArgumentNullException(nameof(supplierCreateDto), "SupplierCreateDto cannot be null.");
            }

            try
            {
                var command = _mapper.Map<CreateSupplierCommand>(supplierCreateDto);
                var supplierReadDto = await _mediator.Send(command);

                _logger.LogInformation("Supplier created successfully: {SupplierName}", supplierCreateDto.Name);
                return supplierReadDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the supplier: {SupplierName}", supplierCreateDto.Name);
                throw;
            }
        }

        public async Task<SupplierReadDto> UpdateSupplierAsync(int supplierId, SupplierUpdateDto supplierUpdateDto)
        {
            if (supplierUpdateDto == null)
            {
                _logger.LogWarning("Received null data for supplier update.");
                throw new ArgumentNullException(nameof(supplierUpdateDto), "SupplierUpdateDto cannot be null.");
            }

            try
            {
                var command = new UpdateSupplierCommand(supplierId, supplierUpdateDto);
                var updatedSupplierReadDto = await _mediator.Send(command);

                _logger.LogInformation("Supplier with ID {SupplierId} updated successfully.", supplierId);
                return updatedSupplierReadDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating supplier with ID {SupplierId}.", supplierId);
                throw;
            }
        }

        public async Task<SupplierReadDto> GetSupplierByIdAsync(int supplierId)
        {
            try
            {
                var query = new GetSupplierByIdQuery(supplierId);
                var supplier = await _mediator.Send(query);

                _logger.LogInformation("Supplier with ID {SupplierId} retrieved successfully.", supplierId);
                return supplier;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Supplier with ID {SupplierId} not found.", supplierId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving supplier with ID {SupplierId}.", supplierId);
                throw;
            }
        }

        public async Task<IEnumerable<SupplierReadDto>> GetAllSuppliersAsync()
        {
            try
            {
                var query = new GetAllSuppliersQuery();
                var suppliers = await _mediator.Send(query);

                _logger.LogInformation("All suppliers retrieved successfully.");
                return suppliers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all suppliers.");
                throw;
            }
        }

        public async Task<bool> DeleteSupplierAsync(int supplierId)
        {
            try
            {
                var command = new DeleteSupplierCommand(supplierId);
                var result = await _mediator.Send(command);

                if (result)
                {
                    _logger.LogInformation("Supplier with ID {SupplierId} deactivated successfully.", supplierId);
                }
                else
                {
                    _logger.LogWarning("Failed to deactivate supplier with ID {SupplierId}.", supplierId);
                }

                return result;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Supplier with ID {SupplierId} not found.", supplierId);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Supplier with ID {SupplierId} has linked ingredients.", supplierId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deactivating supplier with ID {SupplierId}.", supplierId);
                throw;
            }
        }

    }
}
