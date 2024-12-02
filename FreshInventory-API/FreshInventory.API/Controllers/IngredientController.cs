using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Exceptions;

namespace FreshInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngredientController(IIngredientService ingredientService, ILogger<IngredientController> logger) : ControllerBase
{
    private readonly IIngredientService _ingredientService = ingredientService;
    private readonly ILogger<IngredientController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> CreateIngredient([FromBody] IngredientCreateDto ingredientDto)
    {
        if (ingredientDto == null)
        {
            _logger.LogWarning("Received null data for creating ingredient.");
            return BadRequest(new { message = "Invalid data." });
        }

        try
        {
            var createdIngredient = await _ingredientService.CreateIngredientAsync(ingredientDto);
            _logger.LogInformation("Ingredient created successfully: {IngredientName}", ingredientDto.Name);
            return CreatedAtAction(nameof(GetIngredientById), new { id = createdIngredient.Id }, createdIngredient);
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception while creating ingredient: {IngredientName}", ingredientDto.Name);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating ingredient: {IngredientName}", ingredientDto.Name);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIngredientById(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Invalid ingredient ID received.");
            return BadRequest(new { message = "Invalid ingredient ID." });
        }

        try
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);

            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {IngredientId} not found.", id);
                return NotFound(new { message = "Ingredient not found." });
            }

            _logger.LogInformation("Ingredient with ID {IngredientId} retrieved successfully.", id);
            return Ok(ingredient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving ingredient with ID {IngredientId}.", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllIngredients()
    {
        try
        {
            var ingredients = await _ingredientService.GetAllIngredientsAsync();
            _logger.LogInformation("All ingredients retrieved successfully.");
            return Ok(ingredients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all ingredients.");
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientUpdateDto ingredientDto)
    {
        if (ingredientDto == null)
        {
            _logger.LogWarning("Received null data for updating ingredient with ID {IngredientId}.", id);
            return BadRequest(new { message = "Invalid data." });
        }

        try
        {
            var updatedIngredient = await _ingredientService.UpdateIngredientAsync(id, ingredientDto);
            _logger.LogInformation("Ingredient with ID {IngredientId} updated successfully.", id);
            return Ok(updatedIngredient);
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception while updating ingredient with ID {IngredientId}.", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating ingredient with ID {IngredientId}.", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Invalid ingredient ID received for deletion.");
            return BadRequest(new { message = "Invalid ingredient ID." });
        }

        try
        {
            await _ingredientService.DeleteIngredientAsync(id);
            _logger.LogInformation("Ingredient with ID {IngredientId} deleted successfully.", id);
            return NoContent();
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception while deleting ingredient with ID {IngredientId}.", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting ingredient with ID {IngredientId}.", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }
}
