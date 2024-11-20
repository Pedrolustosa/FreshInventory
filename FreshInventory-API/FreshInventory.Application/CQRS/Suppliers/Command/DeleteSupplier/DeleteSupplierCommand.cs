using MediatR;

namespace FreshInventory.Application.CQRS.Suppliers.Command.DeleteSupplier
{
    public class DeleteSupplierCommand : IRequest
    {
        public int Id { get; set; }
    }
}
