using FreshInventory.Domain.Enums;

namespace FreshInventory.Application.DTO.IngredientDTO
{
    public class IngredientUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Unit Unit { get; set; }
        public decimal UnitCost { get; set; }
        public Category Category { get; set; }
        public int SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsPerishable { get; set; }
        public int ReorderLevel { get; set; }
    }
}
