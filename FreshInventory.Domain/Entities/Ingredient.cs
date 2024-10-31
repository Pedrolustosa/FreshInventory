namespace FreshInventory.Domain.Entities;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }
}
