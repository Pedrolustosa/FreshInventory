using FreshInventory.Application.DTO.IngredientDTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Ingredient.Commands
{
    public class CreateIngredientCommand : IRequest<IngredientReadDto>
    {
        public IngredientCreateDto IngredientCreateDto { get; set; }

        public CreateIngredientCommand(IngredientCreateDto ingredientCreateDto)
        {
            IngredientCreateDto = ingredientCreateDto;
        }
    }
}