using MediatR;
using System.Text;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.Common;
using FreshInventory.Application.Exceptions;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.CQRS.Ingredients.Queries.GetAllIngredients;
using FreshInventory.Application.CQRS.Ingredients.Queries.GetIngredientById;
using FreshInventory.Application.CQRS.Ingredients.Commands.CreateIngredient;
using FreshInventory.Application.CQRS.Ingredients.Commands.UpdateIngredient;
using FreshInventory.Application.CQRS.Ingredients.Commands.DeleteIngredient;
using FreshInventory.Application.DTO.IngredientDTO;

namespace FreshInventory.Application.Services;

public class IngredientService(
    IMediator mediator,
    ILogger<IngredientService> logger,
    IEmailService emailService) : IIngredientService
{
    private readonly IMediator _mediator = mediator;
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<IngredientService> _logger = logger;

    public async Task<PagedList<IngredientDto>> GetAllIngredientsAsync(
        int pageNumber, int pageSize, string? name = null, string? category = null, string? sortBy = null, string? sortDirection = null)
    {
        try
        {
            var query = new GetAllIngredientsQuery(pageNumber, pageSize, name, category, sortBy, sortDirection);
            var pagedIngredients = await _mediator.Send(query);

            _logger.LogInformation("Ingredients retrieved successfully.");
            return pagedIngredients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving ingredients.");
            throw new ServiceException("An error occurred while retrieving ingredients.", ex);
        }
    }

    public async Task<IngredientDto> GetIngredientByIdAsync(int id)
    {
        try
        {
            var query = new GetIngredientByIdQuery(id);
            var ingredientDto = await _mediator.Send(query) ?? throw new ServiceException($"Ingredient with ID {id} not found.");
            _logger.LogInformation("Ingredient with ID {Id} retrieved successfully.", id);
            return ingredientDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving ingredient with ID {Id}.", id);
            throw new ServiceException("An error occurred while retrieving the ingredient.", ex);
        }
    }

    public async Task<IngredientDto> AddIngredientAsync(IngredientCreateDto ingredientCreateDto)
    {
        try
        {
            var command = new CreateIngredientCommand(ingredientCreateDto);
            var createdIngredientDto = await _mediator.Send(command);

            _logger.LogInformation("Ingredient '{Name}' added successfully.", createdIngredientDto.Name);

            var subject = $"New Ingredient Added: {createdIngredientDto.Name}";
            var body = GenerateCreateEmailBody(createdIngredientDto);
            await _emailService.SendEmailAsync(subject, body);

            return createdIngredientDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding ingredient '{Name}'.", ingredientCreateDto.Name);
            throw new ServiceException("An error occurred while adding the ingredient.", ex);
        }
    }

    public async Task UpdateIngredientAsync(IngredientUpdateDto ingredientUpdateDto)
    {
        try
        {
            var oldIngredientDto = await GetIngredientByIdAsync(ingredientUpdateDto.Id);

            var command = new UpdateIngredientCommand(ingredientUpdateDto);
            await _mediator.Send(command);

            _logger.LogInformation("Ingredient '{Name}' updated successfully.", ingredientUpdateDto.Name);

            var newIngredientDto = await GetIngredientByIdAsync(ingredientUpdateDto.Id);

            var subject = $"Ingredient Updated: {ingredientUpdateDto.Name}";
            var body = GenerateUpdateEmailBody(oldIngredientDto, newIngredientDto);
            await _emailService.SendEmailAsync(subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating ingredient '{Name}'.", ingredientUpdateDto.Name);
            throw new ServiceException("An error occurred while updating the ingredient.", ex);
        }
    }

    public async Task DeleteIngredientAsync(int id)
    {
        try
        {
            var ingredientDto = await GetIngredientByIdAsync(id);

            var command = new DeleteIngredientCommand(id);
            await _mediator.Send(command);

            _logger.LogInformation("Ingredient with ID {Id} deleted successfully.", id);

            var subject = $"Ingredient Deleted: {ingredientDto.Name}";
            var body = GenerateDeleteEmailBody(ingredientDto);
            await _emailService.SendEmailAsync(subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting ingredient with ID {Id}.", id);
            throw new ServiceException("An error occurred while deleting the ingredient.", ex);
        }
    }

    private static string GenerateCreateEmailBody(IngredientDto ingredient)
    {
        return $@"
            <html>
            <body>
                <h2>New Ingredient Added</h2>
                <p>The following ingredient has been added:</p>
                <table>
                    <tr><td><strong>Name:</strong></td><td>{ingredient.Name}</td></tr>
                    <tr><td><strong>Quantity:</strong></td><td>{ingredient.Quantity} {ingredient.Unit}</td></tr>
                    <tr><td><strong>Unit Cost:</strong></td><td>{ingredient.UnitCost:C}</td></tr>
                    <tr><td><strong>Category:</strong></td><td>{ingredient.Category}</td></tr>
                    <tr><td><strong>Supplier:</strong></td><td>{ingredient.Supplier}</td></tr>
                    <tr><td><strong>Purchase Date:</strong></td><td>{ingredient.PurchaseDate:yyyy-MM-dd}</td></tr>
                    <tr><td><strong>Expiry Date:</strong></td><td>{ingredient.ExpiryDate:yyyy-MM-dd}</td></tr>
                    <tr><td><strong>Is Perishable:</strong></td><td>{ingredient.IsPerishable}</td></tr>
                    <tr><td><strong>Reorder Level:</strong></td><td>{ingredient.ReorderLevel}</td></tr>
                </table>
            </body>
            </html>";
    }

    private static string GenerateUpdateEmailBody(IngredientDto oldData, IngredientDto newData)
    {
        var differences = GetDifferences(oldData, newData);
        return $@"
            <html>
            <body>
                <h2>Ingredient Updated</h2>
                <p>The ingredient <strong>{oldData.Name}</strong> has been updated.</p>
                <h3>Changes:</h3>
                <table>
                    <tr>
                        <th>Field</th>
                        <th>Old Value</th>
                        <th>New Value</th>
                    </tr>
                    {differences}
                </table>
            </body>
            </html>";
    }

    private static string GenerateDeleteEmailBody(IngredientDto ingredient)
    {
        return $@"
            <html>
            <body>
                <h2>Ingredient Deleted</h2>
                <p>The following ingredient has been deleted:</p>
                <table>
                    <tr><td><strong>Name:</strong></td><td>{ingredient.Name}</td></tr>
                    <tr><td><strong>Quantity:</strong></td><td>{ingredient.Quantity} {ingredient.Unit}</td></tr>
                    <tr><td><strong>Unit Cost:</strong></td><td>{ingredient.UnitCost:C}</td></tr>
                    <tr><td><strong>Category:</strong></td><td>{ingredient.Category}</td></tr>
                    <tr><td><strong>Supplier:</strong></td><td>{ingredient.Supplier}</td></tr>
                    <tr><td><strong>Purchase Date:</strong></td><td>{ingredient.PurchaseDate:yyyy-MM-dd}</td></tr>
                    <tr><td><strong>Expiry Date:</strong></td><td>{ingredient.ExpiryDate:yyyy-MM-dd}</td></tr>
                    <tr><td><strong>Is Perishable:</strong></td><td>{ingredient.IsPerishable}</td></tr>
                    <tr><td><strong>Reorder Level:</strong></td><td>{ingredient.ReorderLevel}</td></tr>
                </table>
            </body>
            </html>";
    }

    private static string GetDifferences(IngredientDto oldData, IngredientDto newData)
    {
        var differences = new StringBuilder();

        void AddDifference(string fieldName, object oldValue, object newValue)
        {
            differences.AppendLine($"<tr><td><strong>{fieldName}</strong></td><td>{oldValue}</td><td>{newValue}</td></tr>");
        }

        if (oldData.Name != newData.Name)
            AddDifference("Name", oldData.Name, newData.Name);

        if (oldData.Quantity != newData.Quantity)
            AddDifference("Quantity", oldData.Quantity, newData.Quantity);

        if (oldData.Unit != newData.Unit)
            AddDifference("Unit", oldData.Unit, newData.Unit);

        if (oldData.UnitCost != newData.UnitCost)
            AddDifference("Unit Cost", oldData.UnitCost.ToString("C"), newData.UnitCost.ToString("C"));

        if (oldData.Category != newData.Category)
            AddDifference("Category", oldData.Category, newData.Category);

        if (oldData.Supplier != newData.Supplier)
            AddDifference("Supplier", oldData.Supplier, newData.Supplier);

        if (oldData.PurchaseDate != newData.PurchaseDate)
            AddDifference("Purchase Date", oldData.PurchaseDate.ToString("yyyy-MM-dd"), newData.PurchaseDate.ToString("yyyy-MM-dd"));

        if (oldData.ExpiryDate != newData.ExpiryDate)
            AddDifference("Expiry Date", oldData.ExpiryDate.ToString("yyyy-MM-dd"), newData.ExpiryDate.ToString("yyyy-MM-dd"));

        if (oldData.IsPerishable != newData.IsPerishable)
            AddDifference("Is Perishable", oldData.IsPerishable, newData.IsPerishable);

        if (oldData.ReorderLevel != newData.ReorderLevel)
            AddDifference("Reorder Level", oldData.ReorderLevel, newData.ReorderLevel);

        return differences.ToString();
    }
}
