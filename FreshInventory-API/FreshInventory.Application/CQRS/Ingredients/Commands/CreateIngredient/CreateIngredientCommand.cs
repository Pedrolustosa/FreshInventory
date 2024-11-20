using FreshInventory.Application.DTO.IngredientDTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommand : IRequest<IngredientDto>
    {
        public IngredientCreateDto IngredientCreateDto { get; }

        public CreateIngredientCommand(IngredientCreateDto ingredientCreateDto)
        {
            IngredientCreateDto = ingredientCreateDto;
        }
    }
}
