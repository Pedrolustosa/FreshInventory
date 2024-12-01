namespace FreshInventory.Application.DTO.UserDTO
{
    public class UserReadDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Bio { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public int Gender { get; set; }
        public string Nationality { get; set; }
        public string LanguagePreference { get; set; }
        public string TimeZone { get; set; }
    }
}
