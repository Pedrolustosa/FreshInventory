using System.ComponentModel.DataAnnotations;

namespace FreshInventory.Application.DTO.SupplierDTO
{
    public class SupplierCreateDto
    {
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Phone field is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The Address field is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The ContactPerson field is required.")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "The Category field is required.")]
        public string Category { get; set; }

        public bool Status { get; set; } = true;
    }
}
