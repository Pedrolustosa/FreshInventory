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

            builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Contact).HasMaxLength(100);
            builder.Property(s => s.Email).HasMaxLength(100);
            builder.Property(s => s.Phone).HasMaxLength(15);

            builder.HasMany(s => s.Ingredients)
                .WithOne(i => i.Supplier)
                .HasForeignKey(i => i.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Suppliers");
        }
    }
}
