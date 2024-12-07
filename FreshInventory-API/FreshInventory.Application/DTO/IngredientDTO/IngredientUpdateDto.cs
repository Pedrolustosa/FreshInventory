namespace FreshInventory.Application.DTO.IngredientDTO
{
    public class IngredientUpdateDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public int SupplierId { get; set; }
    }
}
