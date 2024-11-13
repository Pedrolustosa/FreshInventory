using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Infrastructure.Data.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<IngredientRecipe> IngredientRecipes { get; set; }

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

            // Define relacionamento com Supplier
            entity.HasOne(e => e.Supplier)
                  .WithMany()
                  .HasForeignKey("SupplierId")
                  .IsRequired();
        });

        // Configuração da entidade Supplier
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.ContactInfo)
                .HasMaxLength(200);

            entity.Property(e => e.Address)
                .HasMaxLength(300);

            entity.Property(e => e.CreatedDate)
                .IsRequired();

            entity.Property(e => e.UpdatedDate)
                .IsRequired();
        });

        // Configuração da entidade Recipe
        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedDate)
                .IsRequired();

            entity.Property(e => e.UpdatedDate)
                .IsRequired();
        });

        modelBuilder.Entity<IngredientRecipe>(entity =>
        {
            entity.HasKey(e => new { e.IngredientId, e.RecipeId });

            entity.Property(e => e.Quantity)
                .IsRequired();

            entity.HasOne(ir => ir.Ingredient)
                .WithMany(i => i.IngredientRecipes)
                .HasForeignKey(ir => ir.IngredientId);

            entity.HasOne(ir => ir.Recipe)
                .WithMany(r => r.IngredientRecipes)
                .HasForeignKey(ir => ir.RecipeId);
        });
    }
}
