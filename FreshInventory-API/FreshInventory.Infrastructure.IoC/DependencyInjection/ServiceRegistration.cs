using Serilog;
using MediatR;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using FreshInventory.Application.Mappings;
using FreshInventory.Application.Services;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.Validators;
using FreshInventory.Application.DTO.EmailDTO;
using Microsoft.Extensions.DependencyInjection;
using FreshInventory.Infrastructure.Data.Context;
using FreshInventory.Infrastructure.Data.Services;

namespace FreshInventory.Infrastructure.IoC.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // MediatR configuration - registers all handlers in the Application assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IngredientProfile).Assembly));

        // AutoMapper profiles for Ingredient, Recipe, RecipeIngredient
        services.AddAutoMapper(typeof(IngredientProfile), typeof(RecipeProfile), typeof(UserProfile), typeof(SupplierProfile), typeof(RecipeIngredientProfile));

        // FluentValidation validators
        services.AddValidatorsFromAssemblyContaining<IngredientCreateDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<IngredientUpdateDtoValidator>();
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        // Database context configuration
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        // Email configuration
        var emailConfigSection = configuration.GetSection("EmailConfiguration");
        var emailConfig = emailConfigSection.Get<EmailSettingsDto>();
        if (emailConfig != null)
            services.AddSingleton(emailConfig);
        else
            throw new Exception("EmailConfiguration section is missing or invalid in appsettings.json");

        // Services and repositories
        services.AddScoped<IIngredientService, IngredientService>();
        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();

        services.AddScoped<IEmailService, EmailService>();

        // Serilog configuration
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
