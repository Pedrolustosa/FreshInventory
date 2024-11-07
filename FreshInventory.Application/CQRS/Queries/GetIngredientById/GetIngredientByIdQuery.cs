using MediatR;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.CQRS.Queries.GetIngredientById;

public class GetIngredientByIdQuery(int id) : IRequest<IngredientDto>
{
    public int Id { get; set; } = id;
}
