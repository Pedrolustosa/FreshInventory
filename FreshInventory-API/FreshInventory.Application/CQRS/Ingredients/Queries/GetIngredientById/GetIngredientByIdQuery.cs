using MediatR;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.CQRS.Ingredients.Queries.GetIngredientById;

public class GetIngredientByIdQuery(int id) : IRequest<IngredientDto>
{
    public int Id { get; } = id;
}
