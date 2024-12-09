using FreshInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshInventory.Infrastructure.Data.Configurations
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            // Configurando a chave primária
            builder.HasKey(r => r.Id);

            // Configurando propriedades básicas
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .HasMaxLength(1000);

            builder.Property(r => r.Servings)
                .IsRequired();

            builder.Property(r => r.PreparationTime)
                .IsRequired();

            // Configurando Steps como uma lista serializada
            builder.Property(r => r.Steps)
                .HasConversion(
                    steps => string.Join("||", steps),
                    steps => steps.Split("||", StringSplitOptions.None).ToList()
                )
                .HasColumnType("TEXT")
                .IsRequired();

            // Configurando relacionamento com RecipeIngredient
            builder.HasMany(r => r.RecipeIngredients)
                .WithOne(ri => ri.Recipe)
                .HasForeignKey(ri => ri.RecipeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Configurando o nome da tabela
            builder.ToTable("Recipes");
        }
    }
}
