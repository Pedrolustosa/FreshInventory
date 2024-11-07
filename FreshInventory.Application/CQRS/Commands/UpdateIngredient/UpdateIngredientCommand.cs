using MediatR;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.CQRS.Commands.UpdateIngredient;

public class UpdateIngredientCommand(IngredientUpdateDto ingredient) : IRequest
{
    public IngredientUpdateDto Ingredient { get; set; } = ingredient;
}
