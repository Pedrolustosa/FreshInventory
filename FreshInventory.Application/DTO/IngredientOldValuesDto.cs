namespace FreshInventory.Application.DTO;

public class IngredientOldValuesDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required decimal Quantity { get; set; }
    public required string Unit { get; set; }
    public required decimal UnitCost { get; set; }
    public required string Category { get; set; }
    public required string Supplier { get; set; }
    public required DateTime PurchaseDate { get; set; }
    public required DateTime ExpiryDate { get; set; }
    public required bool IsPerishable { get; set; }
    public required decimal ReorderLevel { get; set; }
}
