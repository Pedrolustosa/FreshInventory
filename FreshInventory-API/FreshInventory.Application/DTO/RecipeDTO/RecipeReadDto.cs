public class RecipeReadDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Servings { get; set; }
    public TimeSpan PreparationTime { get; set; }
    public List<RecipeIngredientDto> Ingredients { get; set; }
    public List<string> Steps { get; set; }
}

public class RecipeIngredientDto
{
    public int IngredientId { get; set; }
    public string IngredientName { get; set; }
    public int Quantity { get; set; }
}
