using MediatR;
using Microsoft.EntityFrameworkCore;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using FreshInventory.Application.Mappings;
using FreshInventory.Application.Services;
using FreshInventory.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using FreshInventory.Infrastructure.Data.Context;
using FreshInventory.Infrastructure.Data.Services;
using FreshInventory.Application.CQRS.Commands.CreateIngredient;

namespace FreshInventory.Infrastructure.IoC.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateIngredientCommandHandler).Assembly));
        services.AddAutoMapper(typeof(IngredientProfile));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailSettingsDto>();
        services.AddSingleton(emailConfig);

        services.AddScoped<IIngredientRepository, IngredientRepository>();

        services.AddScoped<IIngredientService, IngredientService>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
