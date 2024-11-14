using MediatR;

namespace FreshInventory.Application.CQRS.Ingredients.Commands.DeleteIngredient
{
    public class DeleteIngredientCommand(int id) : IRequest
    {
        public int Id { get; } = id;
    }
}
