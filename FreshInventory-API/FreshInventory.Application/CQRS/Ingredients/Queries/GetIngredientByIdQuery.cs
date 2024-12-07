using FreshInventory.Application.DTO.IngredientDTO;
using MediatR;

namespace FreshInventory.Application.Features.Ingredients.Queries
{
    public class GetIngredientByIdQuery : IRequest<IngredientReadDto>
    {
        public int IngredientId { get; set; }

        public GetIngredientByIdQuery(int ingredientId)
        {
            IngredientId = ingredientId;
        }
    }
}
