using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.DTO;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.Common;

namespace FreshInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController(IRecipeService recipeService) : ControllerBase
    {
        private readonly IRecipeService _recipeService = recipeService;

        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedList<RecipeDto>>> GetAll(
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
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeCreateDto recipeCreateDto)
        {
            var createdRecipe = await _recipeService.CreateRecipeAsync(recipeCreateDto);
            return CreatedAtAction(nameof(GetRecipeById), new { id = createdRecipe.Id }, createdRecipe);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDto recipeUpdateDto)
        {
            if (id != recipeUpdateDto.Id)
                return BadRequest("Recipe ID mismatch");

            var updatedRecipe = await _recipeService.UpdateRecipeAsync(recipeUpdateDto);
            return Ok(updatedRecipe);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            return recipe != null ? Ok(recipe) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            await _recipeService.DeleteRecipeAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/reactivate")]
        public async Task<IActionResult> ReactivateRecipe(int id)
        {
            await _recipeService.ReactivateRecipeAsync(id);
            return Ok();
        }

        [HttpPost("{id}/reserve-ingredients")]
        public async Task<IActionResult> ReserveIngredients(int id)
        {
            var result = await _recipeService.ReserveIngredientsAsync(id);
            return result ? Ok("Ingredients reserved") : BadRequest("Failed to reserve ingredients");
        }
    }
}
