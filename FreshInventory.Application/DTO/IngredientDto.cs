namespace FreshInventory.Application.DTO;

public class IngredientDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
}
