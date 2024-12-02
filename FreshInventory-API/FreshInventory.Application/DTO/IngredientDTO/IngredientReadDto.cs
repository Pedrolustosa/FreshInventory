using FreshInventory.Domain.Enums;

namespace FreshInventory.Application.DTO.IngredientDTO
{
    public class IngredientReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Unit Unit { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public Category Category { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsPerishable { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
