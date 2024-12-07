using MediatR;

namespace FreshInventory.Application.Features.Suppliers.Commands
{
    public class DeleteSupplierCommand : IRequest<bool>
    {
        public int SupplierId { get; }

        public DeleteSupplierCommand(int supplierId)
        {
            SupplierId = supplierId;
        }
    }
}
