using MediatR;

namespace FreshInventory.Application.CQRS.Suppliers.Command.UpdateSupplier
{
    public class UpdateSupplierCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Category { get; set; }
        public bool Status { get; set; }
    }
}
