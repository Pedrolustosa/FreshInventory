using MediatR;
using FreshInventory.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Suppliers.Commands;

public class DeleteSupplierCommandHandler(ISupplierRepository supplierRepository, ILogger<DeleteSupplierCommandHandler> logger) : IRequestHandler<DeleteSupplierCommand, bool>
{
    private readonly ISupplierRepository _supplierRepository = supplierRepository;
    private readonly ILogger<DeleteSupplierCommandHandler> _logger = logger;

    public async Task<bool> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Attempting to deactivate supplier with ID {SupplierId}.", request.SupplierId);

            var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {SupplierId} not found.", request.SupplierId);
                throw new KeyNotFoundException($"Supplier with ID {request.SupplierId} not found.");
            }

            var hasLinkedIngredients = await _supplierRepository.HasLinkedIngredientsAsync(request.SupplierId);
            if (hasLinkedIngredients)
            {
                _logger.LogWarning("Cannot deactivate supplier with ID {SupplierId} because it has linked ingredients.", request.SupplierId);
                throw new InvalidOperationException("Cannot deactivate supplier with linked ingredients.");
            }

            var result = await _supplierRepository.DeleteAsync(request.SupplierId);

            if (result)
            {
                _logger.LogInformation("Supplier with ID {SupplierId} deactivated successfully.", request.SupplierId);
            }
            else
            {
                _logger.LogWarning("Failed to deactivate supplier with ID {SupplierId}.", request.SupplierId);
            }

            return result;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Error while attempting to deactivate supplier with ID {SupplierId}.", request.SupplierId);
            throw;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operation not allowed for supplier with ID {SupplierId}.", request.SupplierId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deactivating supplier with ID {SupplierId}.", request.SupplierId);
            throw;
        }
    }
}
