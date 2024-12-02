using MediatR;

namespace FreshInventory.Application.Features.Ingredients.Commands
{
    public class DeleteIngredientCommand : IRequest<bool>
    {
        public int IngredientId { get; set; }

        public DeleteIngredientCommand(int ingredientId)
        {
            IngredientId = ingredientId;
        }
    }
}
