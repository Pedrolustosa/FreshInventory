﻿namespace FreshInventory.Application.DTO.RecipeDTO
{
    public class RecipeCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string PreparationTime { get; set; }
        public string Servings { get; set; }
        public bool IsAvailable { get; set; }
        public List<string> Instructions { get; set; } = new List<string>();
        public List<RecipeIngredientCreateDto> Ingredients { get; set; } = new List<RecipeIngredientCreateDto>();
    }
}