namespace FreshInventory.Application.DTO.IngredientDTO
{
    public class IngredientCreateDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public int SupplierId { get; set; }
    }
}
