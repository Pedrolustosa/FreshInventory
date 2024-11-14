using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Infrastructure.Data.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Quantity)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.Unit)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.UnitCost)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Category)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.PurchaseDate)
                .IsRequired();

            entity.Property(e => e.ExpiryDate)
                .IsRequired();

            entity.Property(e => e.IsPerishable)
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(e => e.ReorderLevel)
                .IsRequired()
                .HasDefaultValue(10);

            entity.Property(e => e.CreatedDate)
                .IsRequired();

            entity.Property(e => e.UpdatedDate)
                .IsRequired();
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
            entity.Property(r => r.IsActive).HasDefaultValue(true);

            entity.HasMany(r => r.Ingredients)
                  .WithOne(ri => ri.Recipe)
                  .HasForeignKey(ri => ri.RecipeId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(ri => ri.Id);
            entity.Property(ri => ri.QuantityRequired).IsRequired();
            entity.HasOne(ri => ri.Ingredient)
                  .WithMany()
                  .HasForeignKey(ri => ri.IngredientId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
