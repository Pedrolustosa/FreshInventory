﻿using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FreshInventory.Infrastructure.Data.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.FullName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(u => u.DateOfBirth)
                      .IsRequired(false);
            });

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

                entity.Property(e => e.PurchaseDate).IsRequired();
                entity.Property(e => e.ExpiryDate).IsRequired();
                entity.Property(e => e.IsPerishable).IsRequired().HasDefaultValue(true);
                entity.Property(e => e.ReorderLevel).IsRequired().HasDefaultValue(10);
                entity.Property(e => e.CreatedDate).IsRequired();
                entity.Property(e => e.UpdatedDate).IsRequired();
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(r => r.IsAvailable)
                      .IsRequired()
                      .HasDefaultValue(true);

                entity.Property(r => r.CreatedDate).IsRequired();
                entity.Property(r => r.UpdatedDate).IsRequired();

                entity.HasMany(r => r.Ingredients)
                      .WithOne()
                      .HasForeignKey(ri => ri.RecipeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                // Definir chave composta
                entity.HasKey(ri => new { ri.RecipeId, ri.IngredientId });

                // Configurar propriedades obrigatórias
                entity.Property(ri => ri.Quantity).IsRequired();

                // Relacionamento com Ingredient
                entity.HasOne(ri => ri.Ingredient)
                      .WithMany() // Se Ingredient não tem uma lista de RecipeIngredients
                      .HasForeignKey(ri => ri.IngredientId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relacionamento com Recipe
                entity.HasOne<Recipe>()
                      .WithMany(r => r.Ingredients)
                      .HasForeignKey(ri => ri.RecipeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
