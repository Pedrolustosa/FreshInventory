using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.DTO.IngredientDTO;

namespace FreshInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredient([FromBody] IngredientCreateDto ingredientDto)
        {
            var createdIngredient = await _ingredientService.CreateIngredientAsync(ingredientDto);
            return CreatedAtAction(nameof(GetIngredientById), new { id = createdIngredient.Id }, createdIngredient);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngredientById(int id)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
            if (ingredient == null)
                return NotFound();

            return Ok(ingredient);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _ingredientService.GetAllIngredientsAsync();
            return Ok(ingredients);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientUpdateDto ingredientDto)
        {
            var updatedIngredient = await _ingredientService.UpdateIngredientAsync(id, ingredientDto);
            return Ok(updatedIngredient);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            await _ingredientService.DeleteIngredientAsync(id);
            return NoContent();
        }
    }
}
