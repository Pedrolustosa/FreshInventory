using FreshInventory.Application.DTO.IngredientDTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Ingredient.Commands
{
    public class CreateIngredientCommand : IRequest<IngredientReadDto>
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public int SupplierId { get; set; }

        public CreateIngredientCommand(string name, int quantity, decimal unitCost, int supplierId)
        {
            Name = name;
            Quantity = quantity;
            UnitCost = unitCost;
            SupplierId = supplierId;
        }
    }
}
