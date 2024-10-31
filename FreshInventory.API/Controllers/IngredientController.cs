using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.DTO;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientController(IIngredientService ingredientService) : ControllerBase
{
    private readonly IIngredientService _ingredientService = ingredientService;

    [HttpPost]
    public async Task<IActionResult> AddIngredient([FromBody] IngredientDto ingredientDto)
    {
        await _ingredientService.AddIngredientAsync(ingredientDto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientDto ingredientDto)
    {
        if (id != ingredientDto.Id)
        {
            return BadRequest("Ingredient ID does not match the resource ID.");
        }

        await _ingredientService.UpdateIngredientAsync(ingredientDto);
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
        var ingredientDto = await _ingredientService.GetIngredientByIdAsync(id);
        if (ingredientDto == null)
        {
            return NotFound();
        }

        return Ok(ingredientDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IngredientDto>>> GetAllIngredients()
    {
        var ingredientDtos = await _ingredientService.GetAllIngredientsAsync();
        return Ok(ingredientDtos);
    }
}
