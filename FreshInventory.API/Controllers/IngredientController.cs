using Microsoft.AspNetCore.Mvc;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientController(IIngredientService ingredientService) : ControllerBase
{
    private readonly IIngredientService _ingredientService = ingredientService;

    [HttpPost]
    public async Task<IActionResult> AddIngredient([FromBody] Ingredient ingredient)
    {
        await _ingredientService.AddIngredientAsync(ingredient);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIngredient(int id, [FromBody] Ingredient ingredient)
    {
        if (id != ingredient.Id)
        {
            return BadRequest("Ingredient ID does not match the resource ID.");
        }

        await _ingredientService.UpdateIngredientAsync(ingredient);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        await _ingredientService.DeleteIngredientAsync(id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIngredientById(int id)
    {
        var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
        if (ingredient == null)
        {
            return NotFound();
        }
        return Ok(ingredient);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ingredient>>> GetAllIngredients()
    {
        var ingredients = await _ingredientService.GetAllIngredientsAsync();
        return Ok(ingredients);
    }
}
