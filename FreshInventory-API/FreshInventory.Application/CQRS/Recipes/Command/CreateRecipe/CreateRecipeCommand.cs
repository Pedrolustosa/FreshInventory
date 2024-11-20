using MediatR;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.CQRS.Recipes.Command.CreateRecipe;

namespace FreshInventory.Application.CQRS.Commands.CreateRecipe;

public class CreateRecipeCommand : IRequest<RecipeDto>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string PreparationTime { get; set; }
    public string Servings { get; set; }
    public bool IsAvailable { get; set; }
    public List<string> Instructions { get; set; }
    public List<CreateRecipeIngredientCommand> Ingredients { get; set; }
}
