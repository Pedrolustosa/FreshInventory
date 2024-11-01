using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.DTO;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientController(IIngredientService ingredientService) : ControllerBase
{
    private readonly IIngredientService _ingredientService = ingredientService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIngredientById(int id)
    {
        var ingredientDto = await _ingredientService.GetIngredientByIdAsync(id);
        if (ingredientDto == null) return NotFound();
        return Ok(ingredientDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IngredientDto>>> GetAllIngredients()
    {
        var ingredientDtos = await _ingredientService.GetAllIngredientsAsync();
        return Ok(ingredientDtos);
    }

    [HttpPost]
    public async Task<IActionResult> AddIngredient([FromBody] IngredientCreateDto ingredientCreateDto)
    {
        await _ingredientService.AddIngredientAsync(ingredientCreateDto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientUpdateDto ingredientUpdateDto)
    {
        if (id != ingredientUpdateDto.Id) return BadRequest("Ingredient ID does not match the resource ID.");
        await _ingredientService.UpdateIngredientAsync(ingredientUpdateDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        await _ingredientService.DeleteIngredientAsync(id);
        return NoContent();
    }
}
