export interface RecipeIngredient {
  ingredientId: number;
  quantity: number;
  unit: string;
}

export interface Recipe {
  id: number;
  name: string;
  description: string;
  category: string;
  preparationTime: number;
  servings: number;
  ingredients: RecipeIngredient[];
  instructions: string[];
  imageUrl?: string;
  isAvailable?: boolean;
  createdDate: Date;
  updatedDate: Date;
}