using FreshInventory.Application.DTO.IngredientDTO;
using MediatR;

namespace FreshInventory.Application.Features.Ingredients.Commands
{
    public class UpdateIngredientCommand : IRequest<IngredientReadDto>
    {
        public int IngredientId { get; set; }
        public IngredientUpdateDto IngredientUpdateDto { get; set; }

        public UpdateIngredientCommand(int ingredientId, IngredientUpdateDto ingredientUpdateDto)
        {
            IngredientId = ingredientId;
            IngredientUpdateDto = ingredientUpdateDto;
        }
    }
}
