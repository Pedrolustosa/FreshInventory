using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Enums;

namespace FreshInventory.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.DateOfBirth)
                .IsRequired(false);

            builder.Property(u => u.Street)
                .HasMaxLength(150)
                .IsRequired(false);

            builder.Property(u => u.City)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(u => u.State)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(u => u.ZipCode)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(u => u.Country)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(u => u.Bio)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(u => u.AlternatePhoneNumber)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(u => u.Gender)
                .HasConversion(
                    g => g.ToString(),
                    g => (Gender)Enum.Parse(typeof(Gender), g))
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(u => u.Nationality)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(u => u.LanguagePreference)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(u => u.TimeZone)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(u => u.CreatedDate)
                .IsRequired();

            builder.Property(u => u.UpdatedDate)
                .IsRequired();
        }
    }
}
