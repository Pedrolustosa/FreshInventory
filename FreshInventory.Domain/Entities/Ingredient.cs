namespace FreshInventory.Domain.Entities;

public class Ingredient
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; } = DateTime.UtcNow;
}
