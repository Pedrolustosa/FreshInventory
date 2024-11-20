export enum Category {
  Vegetables = 0,
  Fruits = 1,
  Meat = 2,
  Dairy = 3,
  Grains = 4,
  Spices = 5,
  Other = 6
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