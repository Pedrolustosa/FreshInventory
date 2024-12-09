// recipe.model.ts

export interface RecipeCreateDto {
  name: string;
  description: string;
  servings: number;
  preparationTime: string;
  ingredients: RecipeIngredientDto[];
  steps: string[];
}

export interface RecipeIngredientDto {
  ingredientId: number;
  quantity: number;
}

export interface RecipeReadDto {
  id: number;
  name: string;
  description: string;
  servings: number;
  preparationTime: string;
  ingredients: RecipeIngredientDto[];
  steps: string[];
}

export interface RecipeUpdateDto {
  name: string;
  description: string;
  servings: number;
  preparationTime: string;
  ingredients: RecipeIngredientDto[];
  steps: string[];
}
