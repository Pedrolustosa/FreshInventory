import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { Ingredient } from '../../../models/ingredient.model';
import { Observable } from 'rxjs';

export abstract class RecipeFormBase {
  recipeForm: FormGroup;
  availableIngredients: Ingredient[] = [];
  categories = [
    'Main Course',
    'Appetizer',
    'Dessert',
    'Beverage',
    'Side Dish'
  ];

  constructor(
    protected recipeService: RecipeService,
    protected ingredientService: IngredientService,
    protected router: Router
  ) {
    this.recipeForm = new FormBuilder().group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      category: ['', Validators.required],
      preparationTime: ['', [Validators.required, Validators.min(1)]],
      servings: ['', [Validators.required, Validators.min(1)]],
      ingredients: new FormArray([]),
      instructions: new FormArray([])
    });

    this.loadAvailableIngredients();
  }

  private loadAvailableIngredients(): void {
    this.ingredientService.getIngredients(1, 100, '', '', 'quantity', 'desc').subscribe({
      next: (response) => {
        this.availableIngredients = response.items.filter((i: Ingredient) => i.quantity > 0);
      },
      error: (error) => {
        console.error('Error loading ingredients:', error);
      }
    });
  }

  get ingredientsArray(): FormArray {
    return this.recipeForm.get('ingredients') as FormArray;
  }

  get instructionsArray(): FormArray {
    return this.recipeForm.get('instructions') as FormArray;
  }

  addIngredient(): void {
    const ingredientGroup = new FormBuilder().group({
      ingredientId: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(0.1)]]
    });
    this.ingredientsArray.push(ingredientGroup);
  }

  removeIngredient(index: number): void {
    this.ingredientsArray.removeAt(index);
  }

  addInstruction(): void {
    const instructionControl = new FormBuilder().control('', Validators.required);
    this.instructionsArray.push(instructionControl);
  }

  removeInstruction(index: number): void {
    this.instructionsArray.removeAt(index);
  }

  getIngredientName(ingredientId: number): string {
    const ingredient = this.availableIngredients.find(i => i.id === ingredientId);
    return ingredient ? ingredient.name : '';
  }

  getMaxAvailableQuantity(ingredientId: number): number {
    const ingredient = this.availableIngredients.find(i => i.id === ingredientId);
    return ingredient ? ingredient.quantity : 0;
  }

  abstract onSubmit(): void;

  protected markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}