using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.DTO;
using FreshInventory.Application.Common;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController(IIngredientService ingredientService, ILogger<IngredientsController> logger) : ControllerBase
{
    private readonly IIngredientService _ingredientService = ingredientService;
    private readonly ILogger<IngredientsController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<PagedList<IngredientDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? name = null,
        [FromQuery] string? category = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortDirection = null)
    {
        try
        {
            var ingredients = await _ingredientService.GetAllIngredientsAsync(
                pageNumber, pageSize, name, category, sortBy, sortDirection);
            return Ok(ingredients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving ingredients.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IngredientDto>> GetById(int id)
    {
        try
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {Id} not found.", id);
                return NotFound($"Ingredient with ID {id} not found.");
            }
            return Ok(ingredient);
        }
        catch (ServiceException ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving ingredient with ID {Id}.", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving ingredient with ID {Id}.", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<IngredientDto>> Create([FromBody] IngredientCreateDto ingredientCreateDto)
    {
        try
        {
            var createdIngredient = await _ingredientService.AddIngredientAsync(ingredientCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = createdIngredient.Id }, createdIngredient);
        }
        catch (ServiceException ex)
        {
            _logger.LogError(ex, "An error occurred while creating ingredient '{Name}'.", ingredientCreateDto.Name);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating ingredient '{Name}'.", ingredientCreateDto.Name);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] IngredientUpdateDto ingredientUpdateDto)
    {
        if (id != ingredientUpdateDto.Id)
        {
            _logger.LogWarning("ID in URL ({UrlId}) does not match ID in body ({BodyId}).", id, ingredientUpdateDto.Id);
            return BadRequest("ID in URL does not match ID in body.");
        }

        try
        {
            await _ingredientService.UpdateIngredientAsync(ingredientUpdateDto);
            return NoContent();
        }
        catch (ServiceException ex)
        {
            _logger.LogError(ex, "An error occurred while updating ingredient '{Name}'.", ingredientUpdateDto.Name);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating ingredient '{Name}'.", ingredientUpdateDto.Name);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _ingredientService.DeleteIngredientAsync(id);
            return NoContent();
        }
        catch (ServiceException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting ingredient with ID {Id}.", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting ingredient with ID {Id}.", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
