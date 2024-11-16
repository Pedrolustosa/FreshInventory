using FreshInventory.Application.DTO.IngredientDTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Ingredients.Commands.CreateIngredient;

public class CreateIngredientCommand(IngredientCreateDto ingredientCreateDto) : IRequest<IngredientDto>
{
    public IngredientCreateDto IngredientCreateDto { get; } = ingredientCreateDto;
}
