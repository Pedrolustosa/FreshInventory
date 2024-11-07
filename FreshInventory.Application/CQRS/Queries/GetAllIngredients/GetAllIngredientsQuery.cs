using MediatR;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.CQRS.Queries.GetAllIngredients;

public class GetAllIngredientsQuery : IRequest<IEnumerable<IngredientDto>> { }
