using System.Collections.Generic;

namespace FreshInventory.Application.DTO.RecipeDTO
{
    public record RecipeCreateDto(string Name, List<RecipeIngredientDto> Ingredients);
}