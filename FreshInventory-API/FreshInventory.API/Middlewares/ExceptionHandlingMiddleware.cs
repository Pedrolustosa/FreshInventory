using FreshInventory.Domain.Exceptions;
using System.Net;
using System.Text.Json;
using FluentValidation;

namespace FreshInventory.API.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RepositoryException rex)
            {
                _logger.LogWarning(rex, "A repository exception occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                var response = new { message = rex.Message };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (UnauthorizedAccessException uex)
            {
                _logger.LogWarning(uex, "An unauthorized access exception occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var response = new { message = uex.Message };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning(vex, "A validation exception occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var response = new { message = "Validation failed.", errors = vex.Errors };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = new { message = "An unexpected error occurred. Please try again later." };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
