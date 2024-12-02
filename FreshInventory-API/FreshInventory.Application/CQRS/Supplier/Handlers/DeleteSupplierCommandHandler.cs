using MediatR;
using FreshInventory.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Suppliers.Commands
{
    public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, bool>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ILogger<DeleteSupplierCommandHandler> _logger;

        public DeleteSupplierCommandHandler(ISupplierRepository supplierRepository, ILogger<DeleteSupplierCommandHandler> logger)
        {
            _supplierRepository = supplierRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
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

            supplier.Deactivate();

            var result = await _supplierRepository.UpdateAsync(supplier);
            if (result)
            {
                _logger.LogInformation("Supplier with ID {SupplierId} deactivated successfully.", request.SupplierId);
            }

            return result;
        }

    }
}
