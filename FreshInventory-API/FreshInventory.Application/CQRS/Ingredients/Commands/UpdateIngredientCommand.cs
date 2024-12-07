using FreshInventory.Application.DTO.IngredientDTO;
using MediatR;

public class UpdateIngredientCommand : IRequest<IngredientReadDto>
{
    public int IngredientId { get; }
    public IngredientUpdateDto IngredientUpdateDto { get; }

    public UpdateIngredientCommand(int ingredientId, IngredientUpdateDto ingredientUpdateDto)
    {
        IngredientId = ingredientId;
        IngredientUpdateDto = ingredientUpdateDto;
    }
}
