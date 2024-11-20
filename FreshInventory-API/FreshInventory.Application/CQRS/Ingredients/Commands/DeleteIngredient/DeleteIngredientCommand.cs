using MediatR;

namespace FreshInventory.Application.CQRS.Ingredients.Commands.DeleteIngredient
{
    public class DeleteIngredientCommand : IRequest
    {
        public int Id { get; }

        public DeleteIngredientCommand(int id)
        {
            Id = id;
        }
    }
}
