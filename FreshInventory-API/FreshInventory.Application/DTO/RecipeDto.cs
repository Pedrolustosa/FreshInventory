using System.Collections.Generic;

namespace FreshInventory.Application.DTO
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }
    }
}