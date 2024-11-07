using MediatR;

namespace FreshInventory.Application.CQRS.Commands.DeleteIngredient;

public class DeleteIngredientCommand(int id) : IRequest
{
    public int Id { get; set; } = id;
}
