using System.ComponentModel.DataAnnotations;

namespace FreshInventory.Application.DTO.SupplierDTO
{
    public class SupplierCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Contact Person cannot exceed 100 characters.")]
        public string ContactPerson { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
        public string Category { get; set; }

        public bool Status { get; set; } = true;
    }
}
