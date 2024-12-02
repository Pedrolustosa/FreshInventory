using MediatR;
using FreshInventory.Application.DTO.IngredientDTO;

namespace FreshInventory.Application.Features.Ingredients.Queries
{
    public class GetAllIngredientsQuery : IRequest<IEnumerable<IngredientReadDto>>
    {
    }
}
