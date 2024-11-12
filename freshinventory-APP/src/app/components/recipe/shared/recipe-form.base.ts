import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { Ingredient } from '../../../models/ingredient.model';

export abstract class RecipeFormBase {
  recipeForm: FormGroup;
  ingredients: Ingredient[] = [];
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

    this.loadIngredients();
  }

  private loadIngredients(): void {
    this.ingredientService.getIngredients().subscribe(response => {
      this.ingredients = response.items;
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
      quantity: ['', [Validators.required, Validators.min(0.1)]],
      unit: ['', Validators.required]
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