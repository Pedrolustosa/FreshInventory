import { Unit } from './enums/unit.enum';
import { Category } from './enums/category.enum';

export interface Ingredient {
  id: number;
  name: string;
  quantity: number;
  unit: Unit;
  unitCost: number;
  category: Category;
  supplier: string;
  purchaseDate: Date;
  expiryDate: Date;
  isPerishable: boolean;
  reorderLevel: number;
  createdDate: Date;
  updatedDate: Date;
}

export interface CreateIngredient {
  name: string;
  quantity: number;
  unit: Unit;
  unitCost: number;
  category: Category;
  supplier: string;
  purchaseDate: Date;
  expiryDate: Date;
  isPerishable: boolean;
  reorderLevel: number;
}

export interface UpdateIngredient extends CreateIngredient {
  id: number;
}