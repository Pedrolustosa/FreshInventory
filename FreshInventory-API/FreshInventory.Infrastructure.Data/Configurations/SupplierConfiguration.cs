using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Infrastructure.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.ContactPerson)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(s => s.Email)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(s => s.Phone)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(s => s.Address)
                .HasMaxLength(250)
                .IsRequired(false);

            builder.Property(s => s.Category)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.Status)
                .IsRequired();

            builder.Property(s => s.CreatedDate)
                .IsRequired();

            builder.Property(s => s.UpdatedDate)
                .IsRequired();

            builder.HasMany(s => s.Ingredients)
                .WithOne(i => i.Supplier)
                .HasForeignKey(i => i.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
