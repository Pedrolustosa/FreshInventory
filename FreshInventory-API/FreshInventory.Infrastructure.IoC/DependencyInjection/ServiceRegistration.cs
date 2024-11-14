using MediatR;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using FreshInventory.Application.Mappings;
using FreshInventory.Application.Services;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.Validators;
using Microsoft.Extensions.DependencyInjection;
using FreshInventory.Infrastructure.Data.Context;
using FreshInventory.Infrastructure.Data.Services;
using Microsoft.Extensions.Logging;
using Serilog;
using FreshInventory.Application.CQRS.Ingredients.Commands.CreateIngredient;

namespace FreshInventory.Infrastructure.IoC.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateIngredientCommandHandler).Assembly));

        services.AddAutoMapper(typeof(IngredientProfile));
        services.AddAutoMapper(typeof(RecipeProfile));
        services.AddAutoMapper(typeof(RecipeIngredientProfile));
        services.AddValidatorsFromAssemblyContaining<IngredientCreateDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<IngredientUpdateDtoValidator>();
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        var emailConfigSection = configuration.GetSection("EmailConfiguration");
        var emailConfig = emailConfigSection.Get<EmailSettingsDto>();
        if (emailConfig != null)
            services.AddSingleton(emailConfig);
        else
            throw new Exception("EmailConfiguration section is missing or invalid in appsettings.json");
        services.AddScoped<IIngredientService, IngredientService>();
        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IEmailService, EmailService>();
        Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
        return services;
    }
}
