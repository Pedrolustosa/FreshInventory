// Program.cs
using AutoMapper;
using FreshInventory.API.Middlewares;
using FreshInventory.Infrastructure.IoC.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog();

// Adicionar serviços ao container.
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddSwaggerWithJwtSupport(builder.Configuration);

// Construir o app
var app = builder.Build();

// Middleware de tratamento de exceções
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt => opt.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Teste de configuração do AutoMapper
//var mapper = app.Services.GetRequiredService<IMapper>();
//mapper.ConfigurationProvider.AssertConfigurationIsValid();

app.Run();
