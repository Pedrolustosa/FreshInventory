using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using FreshInventory.Application.Mappings;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using FreshInventory.Infrastructure.Data.Context;
using FreshInventory.Infrastructure.Data.Services;

namespace FreshInventory.Infrastructure.IoC.DependencyInjection;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(IngredientProfile));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IIngredientService, IngredientService>();
    }
}