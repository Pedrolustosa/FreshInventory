export interface RecipeIngredient {
  ingredientId: number;
  quantity: number;
}

export interface CreateRecipe {
  id: number;
  name: string;
  category: string;
  preparationTime: number;
  servings: number;
  description: string;
  ingredients: RecipeIngredient[];
  instructions: string[];
}

export interface Recipe {
  id: number;
  name: string;
  category: string;
  preparationTime: number;
  servings: number;
  description: string;
  ingredients: RecipeIngredient[];
  instructions: string[];
}