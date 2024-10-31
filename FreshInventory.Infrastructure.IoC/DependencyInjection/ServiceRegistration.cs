using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using FreshInventory.Application.Service;
using FreshInventory.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using FreshInventory.Infrastructure.Data.Context;
using FreshInventory.Infrastructure.Data.Service;

namespace FreshInventory.Infrastructure.IoC.DependencyInjection;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurando o ApplicationDbContext para usar SQLite
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        // Adicionar outras injeções de dependência aqui
        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IIngredientService, IngredientService>();
    }
}