using Microsoft.AspNetCore.Identity;
using FreshInventory.Domain.Enums;

namespace FreshInventory.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Bio { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string Nationality { get; set; }
        public string LanguagePreference { get; set; }
        public string TimeZone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        private User() : base() { }

        public User(
            string userName,
            string email,
            string fullName,
            DateTime? dateOfBirth = null,
            string bio = null,
            Gender? gender = null,
            string nationality = null,
            string languagePreference = null,
            string timeZone = null)
            : base(userName)
        {
            SetEmail(email);
            SetFullName(fullName);
            DateOfBirth = dateOfBirth;
            Bio = bio;
            Gender = (Gender)gender;
            Nationality = nationality;
            LanguagePreference = languagePreference;
            TimeZone = timeZone;
            SecurityStamp = Guid.NewGuid().ToString();
        }

        public void SetFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name cannot be null or empty.", nameof(fullName));

            FullName = fullName;
        }

        public void SetDateOfBirth(DateTime? dateOfBirth)
        {
            if (dateOfBirth.HasValue && dateOfBirth.Value > DateTime.UtcNow)
                throw new ArgumentException("Date of birth cannot be in the future.", nameof(dateOfBirth));

            DateOfBirth = dateOfBirth;
        }

        public void SetBio(string bio)
        {
            Bio = bio;
        }

        public void SetGender(Gender? gender)
        {
            Gender = (Gender)gender;
        }

        public void SetNationality(string nationality)
        {
            Nationality = nationality;
        }

        public void SetLanguagePreference(string language)
        {
            LanguagePreference = language;
        }

        public void SetTimeZone(string timeZone)
        {
            TimeZone = timeZone;
        }

        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            Email = email;
            NormalizedEmail = email.ToUpperInvariant();
        }
    }
}
