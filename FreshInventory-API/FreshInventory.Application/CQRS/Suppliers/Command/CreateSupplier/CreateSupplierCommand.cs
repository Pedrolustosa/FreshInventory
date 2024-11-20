using MediatR;
using FreshInventory.Application.DTO.SupplierDTO;

namespace FreshInventory.Application.CQRS.Suppliers.Command.CreateSupplier
{
    public class CreateSupplierCommand : IRequest<SupplierDto>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Category { get; set; }
        public bool Status { get; set; }
    }
}
