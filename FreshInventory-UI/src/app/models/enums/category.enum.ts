export enum Category {
  Vegetables = 1,
  Fruits = 2,
  Meat = 3,
  Dairy = 4,
  Grains = 5,
  Spices = 6,
  Other = 7
}

export const CategoryLabels: Record<Category, string> = {
  [Category.Vegetables]: 'Vegetables',
  [Category.Fruits]: 'Fruits',
  [Category.Meat]: 'Meat',
  [Category.Dairy]: 'Dairy',
  [Category.Grains]: 'Grains',
  [Category.Spices]: 'Spices',
  [Category.Other]: 'Other'
};