using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.Common;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.DTO.RecipeDTO;

namespace FreshInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipeController(IRecipeService recipeService, ILogger<RecipeController> logger) : ControllerBase
{
    private readonly IRecipeService _recipeService = recipeService;
    private readonly ILogger<RecipeController> _logger = logger;

    [HttpGet("GetAllRecipes")]
    public async Task<ActionResult<PagedList<RecipeDto>>> GetAllRecipes(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? name = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortDirection = null)
    {
        try
        {
            var recipes = await _recipeService.GetAllRecipesAsync(pageNumber, pageSize, name, sortBy, sortDirection);
            return Ok(recipes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving recipes.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("CreateRecipe")]
    public async Task<IActionResult> CreateRecipe([FromBody] RecipeCreateDto recipeCreateDto)
    {
        try
        {
            var createdRecipe = await _recipeService.CreateRecipeAsync(recipeCreateDto);
            return CreatedAtAction(nameof(GetRecipeById), new { id = createdRecipe.Id }, createdRecipe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating recipe '{Name}'.", recipeCreateDto.Name);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPut("UpdateRecipe/{id:int}")]
    public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDto recipeUpdateDto)
    {
        if (id != recipeUpdateDto.Id)
        {
            _logger.LogWarning("Recipe ID mismatch: URL ID ({UrlId}) does not match body ID ({BodyId}).", id, recipeUpdateDto.Id);
            return BadRequest("Recipe ID mismatch");
        }

        try
        {
            var updatedRecipe = await _recipeService.UpdateRecipeAsync(recipeUpdateDto);
            return Ok(updatedRecipe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating recipe with ID {Id}.", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("GetRecipeById/{id:int}")]
    public async Task<IActionResult> GetRecipeById(int id)
    {
        try
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {Id} not found.", id);
                return NotFound($"Recipe with ID {id} not found.");
            }
            return Ok(recipe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving recipe with ID {Id}.", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpDelete("DeleteRecipe/{id:int}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        try
        {
            await _recipeService.DeleteRecipeAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting recipe with ID {Id}.", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("ReactivateRecipe/{id:int}")]
    public async Task<IActionResult> ReactivateRecipe(int id)
    {
        try
        {
            await _recipeService.ReactivateRecipeAsync(id);
            return Ok("Recipe reactivated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while reactivating recipe with ID {Id}.", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("ReserveIngredientsForRecipe/{id:int}")]
    public async Task<IActionResult> ReserveIngredients(int id)
    {
        try
        {
            var result = await _recipeService.ReserveIngredientsAsync(id);
            return result ? Ok("Ingredients reserved successfully") : BadRequest("Failed to reserve ingredients");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while reserving ingredients for recipe with ID {Id}.", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
