using MediatR;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.CQRS.Commands.CreateIngredient;

public class CreateIngredientCommand(IngredientCreateDto ingredient) : IRequest<int>
{
    public IngredientCreateDto Ingredient { get; set; } = ingredient;
}
