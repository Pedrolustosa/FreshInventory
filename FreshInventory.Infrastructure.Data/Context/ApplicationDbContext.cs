using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Infrastructure.Data.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Ingredient> Ingredients { get; set; }

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

                entity.Property(e => e.Supplier)
                    .HasMaxLength(100);

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
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Ingredient>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var now = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetCreatedDate(now);
                }

                entry.Entity.SetUpdatedDate(now);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
