using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.DTO;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientController(IIngredientService ingredientService, ILogger<IngredientController> logger) : ControllerBase
{
    private readonly IIngredientService _ingredientService = ingredientService;
    private readonly ILogger<IngredientController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> AddIngredient([FromBody] IngredientCreateDto ingredientCreateDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for AddIngredient.");
            return BadRequest(ModelState);
        }

        await _ingredientService.AddIngredientAsync(ingredientCreateDto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientUpdateDto ingredientUpdateDto)
    {
        if (id != ingredientUpdateDto.Id)
        {
            _logger.LogWarning("ID in URL does not match ID in body for UpdateIngredient.");
            return BadRequest("Ingredient ID mismatch.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for UpdateIngredient.");
            return BadRequest(ModelState);
        }

        await _ingredientService.UpdateIngredientAsync(ingredientUpdateDto);
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
            _logger.LogWarning("Ingredient with ID {Id} not found.", id);
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
