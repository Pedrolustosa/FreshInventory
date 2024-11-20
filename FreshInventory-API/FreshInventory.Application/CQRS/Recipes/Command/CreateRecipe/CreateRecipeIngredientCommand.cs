using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshInventory.Application.CQRS.Recipes.Command.CreateRecipe
{
    public class CreateRecipeIngredientCommand
    {
        public int IngredientId { get; set; }
        public int Quantity { get; set; }
    }
}
