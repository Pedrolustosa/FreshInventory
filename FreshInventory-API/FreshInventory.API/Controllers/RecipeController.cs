using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.DTO.RecipeDTO;
using Microsoft.AspNetCore.Authorization;

namespace FreshInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipeController(IRecipeService recipeService, ILogger<RecipeController> logger) : ControllerBase
    {
        private readonly IRecipeService _recipeService = recipeService;
        private readonly ILogger<RecipeController> _logger = logger;

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeCreateDto recipeDto)
        {
            if (recipeDto == null)
            {
                _logger.LogWarning("Received null data for recipe creation.");
                return BadRequest(new { message = "Invalid recipe data." });
            }

            try
            {
                var createdRecipe = await _recipeService.CreateRecipeAsync(recipeDto);
                _logger.LogInformation("Recipe created successfully with ID {RecipeId}.", createdRecipe.Id);
                return CreatedAtAction(nameof(GetRecipeById), new { id = createdRecipe.Id }, createdRecipe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the recipe.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid recipe ID received: {RecipeId}", id);
                return BadRequest(new { message = "Invalid recipe ID." });
            }

            try
            {
                var recipe = await _recipeService.GetRecipeByIdAsync(id);
                if (recipe == null)
                {
                    _logger.LogWarning("Recipe with ID {RecipeId} not found.", id);
                    return NotFound(new { message = "Recipe not found." });
                }

                _logger.LogInformation("Recipe with ID {RecipeId} retrieved successfully.", id);
                return Ok(recipe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the recipe with ID {RecipeId}.", id);
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllRecipes()
        {
            try
            {
                var recipes = await _recipeService.GetAllRecipesAsync();
                _logger.LogInformation("All recipes retrieved successfully.");
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all recipes.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDto recipeDto)
        {
            if (recipeDto == null)
            {
                _logger.LogWarning("Received null data for recipe update.");
                return BadRequest(new { message = "Invalid recipe data." });
            }

            try
            {
                var updatedRecipe = await _recipeService.UpdateRecipeAsync(id, recipeDto);
                if (updatedRecipe == null)
                {
                    _logger.LogWarning("Failed to update recipe with ID {RecipeId}.", id);
                    return NotFound(new { message = "Recipe not found." });
                }

                _logger.LogInformation("Recipe with ID {RecipeId} updated successfully.", id);
                return Ok(updatedRecipe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the recipe with ID {RecipeId}.", id);
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid recipe ID received: {RecipeId}", id);
                return BadRequest(new { message = "Invalid recipe ID." });
            }

            try
            {
                var isDeleted = await _recipeService.DeleteRecipeAsync(id);
                if (!isDeleted)
                {
                    _logger.LogWarning("Failed to delete recipe with ID {RecipeId}.", id);
                    return NotFound(new { message = "Recipe not found." });
                }

                _logger.LogInformation("Recipe with ID {RecipeId} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the recipe with ID {RecipeId}.", id);
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
    }
}
