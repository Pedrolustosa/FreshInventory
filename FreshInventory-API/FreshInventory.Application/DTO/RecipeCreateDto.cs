using System.Collections.Generic;

namespace FreshInventory.Application.DTO
{
    public record RecipeCreateDto(string Name, List<RecipeIngredientDto> Ingredients);
}