using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Infrastructure.Configurations
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(r => r.Servings)
                .IsRequired();

            builder.Property(r => r.PreparationTime)
                .IsRequired();

            builder.Property(r => r.CreatedDate)
                .IsRequired();

            builder.Property(r => r.UpdatedDate)
                .IsRequired();

            builder.HasMany(r => r.Ingredients)
                .WithOne()
                .HasForeignKey("RecipeId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
