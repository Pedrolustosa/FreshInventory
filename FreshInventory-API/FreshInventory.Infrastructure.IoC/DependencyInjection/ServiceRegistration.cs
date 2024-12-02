using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;
using FreshInventory.Application.Services;
using FreshInventory.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using FluentValidation;
using FreshInventory.Infrastructure.Data.Services;
using FreshInventory.Application.Features.Users.Handlers;
using FreshInventory.Application.Features.Users.Validators;
using FreshInventory.Application.Profiles;
using FreshInventory.Infrastructure.Data.Repositories;

namespace FreshInventory.Infrastructure.IoC.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            services.AddDbContext<FreshInventoryDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                typeof(CreateUserCommandHandler).Assembly,
                Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(typeof(UserProfile).Assembly);

            services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();

            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();

            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();

            return services;
        }
    }
}
