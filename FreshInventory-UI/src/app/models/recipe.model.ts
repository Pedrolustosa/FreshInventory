// Interface para o frontend (camelCase)
export interface RecipeIngredient {
  ingredientId: number;
  quantity: number;
}

// Interface para o frontend (camelCase)
export interface CreateRecipe {
  name: string;
  category: string;
  preparationTime: number;
  servings: number;
  description: string;
  ingredients: RecipeIngredient[];
  instructions: string[];
}

// Interface que representa o DTO da API (PascalCase)
export interface RecipeCreateDto {
  Name: string;
  Category: string;
  PreparationTime: string;
  Servings: string;
  Description: string;
  Ingredients: {
    IngredientId: number;
    Quantity: number;
  }[];
  Instructions: string[];
}

// Interface para o frontend (camelCase)
export interface Recipe {
  id: number;
  name: string;
  category: string;
  preparationTime: number;
  servings: number;
  description: string;
  ingredients: RecipeIngredient[];
  instructions: string[];
  isActive?: boolean;
  createdAt?: Date;
  updatedAt?: Date;
}