namespace FreshInventory.Application.DTO.SupplierDTO
{
    public class SupplierCreateDto
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public bool Status { get; set; } = true;
    }
}
