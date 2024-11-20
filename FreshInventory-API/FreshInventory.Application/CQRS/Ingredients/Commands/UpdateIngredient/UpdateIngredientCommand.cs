using FreshInventory.Application.DTO.IngredientDTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Ingredients.Commands.UpdateIngredient
{
    public class UpdateIngredientCommand : IRequest
    {
        public IngredientUpdateDto IngredientUpdateDto { get; }

        public UpdateIngredientCommand(IngredientUpdateDto ingredientUpdateDto)
        {
            IngredientUpdateDto = ingredientUpdateDto;
        }
    }
}
