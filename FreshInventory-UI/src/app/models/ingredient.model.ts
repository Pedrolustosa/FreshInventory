export interface IngredientCreateDto {
  name: string;
  quantity: number;
  unitCost: number;
  supplierId: number;
}

export interface IngredientReadDto {
  id: number;
  name: string;
  quantity: number;
  unitCost: number;
  totalCost: number;
  supplierId: number;
  supplierName: string;
  createdDate: Date;
  updatedDate: Date;
}

export interface IngredientUpdateDto {
  name: string;
  quantity: number;
  unitCost: number;
  supplierId: number;
}

export interface RecipeIngredientReadDto {
  ingredientId: number;
  ingredientName: string;
  quantity: number;
}