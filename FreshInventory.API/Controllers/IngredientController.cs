using MediatR;
using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.DTO;
using FreshInventory.Application.CQRS.Commands.CreateIngredient;
using FreshInventory.Application.CQRS.Commands.DeleteIngredient;
using FreshInventory.Application.CQRS.Commands.UpdateIngredient;
using FreshInventory.Application.CQRS.Queries.GetAllIngredients;
using FreshInventory.Application.CQRS.Queries.GetIngredientById;

namespace FreshInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientController(IMediator mediator, ILogger<IngredientController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<IngredientController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> AddIngredient([FromBody] IngredientCreateDto ingredientCreateDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for AddIngredient.");
            return BadRequest(ModelState);
        }

        var command = new CreateIngredientCommand(ingredientCreateDto);
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetIngredientById), new { id }, null);
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

        var command = new UpdateIngredientCommand(ingredientUpdateDto);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        var command = new DeleteIngredientCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIngredientById(int id)
    {
        var query = new GetIngredientByIdQuery(id);
        var ingredientDto = await _mediator.Send(query);

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
        var query = new GetAllIngredientsQuery();
        var ingredientDtos = await _mediator.Send(query);
        return Ok(ingredientDtos);
    }
}
