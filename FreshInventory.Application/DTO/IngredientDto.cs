using FreshInventory.Domain.Enums;

namespace FreshInventory.Application.DTO;

public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public Unit Unit { get; set; }
    public decimal UnitCost { get; set; }
    public Category Category { get; set; }
    public string Supplier { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsPerishable { get; set; }
    public int ReorderLevel { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
