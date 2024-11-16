using Microsoft.AspNetCore.Identity;

namespace FreshInventory.Domain.Entities
{
    public class User : IdentityUser
    {
        public required string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
