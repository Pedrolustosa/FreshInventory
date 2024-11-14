using MediatR;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.CQRS.Ingredients.Commands.UpdateIngredient;

public class UpdateIngredientCommand(IngredientUpdateDto ingredientUpdateDto) : IRequest
{
    public IngredientUpdateDto IngredientUpdateDto { get; } = ingredientUpdateDto;
}
