using System.ComponentModel.DataAnnotations;

namespace FreshInventory.Application.DTO.SupplierDTO
{
    public class SupplierUpdateDto
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Category { get; set; }
        public bool Status { get; set; }
    }
}
