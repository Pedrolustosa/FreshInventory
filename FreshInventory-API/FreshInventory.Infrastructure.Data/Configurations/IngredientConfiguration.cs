using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Infrastructure.Data.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Quantity)
                .IsRequired();

            builder.Property(i => i.Unit)
                .IsRequired();

            builder.Property(i => i.UnitCost)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.Category)
                .IsRequired();

            builder.Property(i => i.PurchaseDate)
                .IsRequired();

            builder.Property(i => i.ExpiryDate)
                .IsRequired();

            builder.Property(i => i.IsPerishable)
                .IsRequired();

            builder.Property(i => i.ReorderLevel)
                .IsRequired();

            builder.Property(i => i.CreatedDate)
                .IsRequired();

            builder.Property(i => i.UpdatedDate)
                .IsRequired();

            builder.HasOne(i => i.Supplier)
                .WithMany(s => s.Ingredients)
                .HasForeignKey(i => i.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}