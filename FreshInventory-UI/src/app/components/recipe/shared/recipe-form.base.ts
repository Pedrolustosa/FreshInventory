import {
  FormBuilder,
  FormGroup,
  FormArray,
  Validators,
  AbstractControl,
  ValidatorFn,
  ValidationErrors,
} from "@angular/forms";
import { Router } from "@angular/router";
import { RecipeService } from "../../../services/recipe.service";
import { IngredientService } from "../../../services/ingredient.service";
import { Ingredient } from "../../../models/ingredient.model";

export abstract class RecipeFormBase {
  recipeForm: FormGroup;
  availableIngredients: Ingredient[] = [];
  categories = ["Main Course", "Appetizer", "Dessert", "Beverage", "Side Dish"];

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
      instructions: new FormArray([], this.minArrayLengthValidator(1)),
      ingredients: new FormArray([], this.minArrayLengthValidator(1))
    });
    

    this.loadAvailableIngredients();
  }

  private loadAvailableIngredients(): void {
    this.ingredientService
      .getIngredients(1, 100, "", "", "quantity", "desc")
      .subscribe({
        next: (response) => {
          this.availableIngredients = response.items.filter(
            (i: Ingredient) => i.quantity > 0
          );
        },
        error: () => {
          console.error("Error loading ingredients");
        },
      });
  }

  get ingredientsArray(): FormArray {
    return this.recipeForm.get("ingredients") as FormArray;
  }

  get instructionsArray(): FormArray {
    return this.recipeForm.get("instructions") as FormArray;
  }

  addIngredient(): void {
    const fb = new FormBuilder();
    const ingredientGroup = fb.group({
      ingredientId: ["", Validators.required],
      quantity: ["", [Validators.required, Validators.min(0.1)]],
    });

    ingredientGroup.valueChanges.subscribe(() => {
      this.recipeForm.updateValueAndValidity();
    });

    this.ingredientsArray.push(ingredientGroup);
  }

  removeIngredient(index: number): void {
    this.ingredientsArray.removeAt(index);
    this.recipeForm.updateValueAndValidity();
  }

  addInstruction(): void {
    const fb = new FormBuilder();
    const instructionControl = fb.control("", Validators.required);

    instructionControl.valueChanges.subscribe(() => {
      this.recipeForm.updateValueAndValidity();
    });

    this.instructionsArray.push(instructionControl);
  }

  removeInstruction(index: number): void {
    this.instructionsArray.removeAt(index);
    this.recipeForm.updateValueAndValidity();
  }

  private minArrayLengthValidator(minLength: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control instanceof FormArray) {
        const valid = control.controls.every((ctrl) => ctrl.valid);
        return control.length >= minLength && valid
          ? null
          : { minArrayLength: { requiredLength: minLength, actualLength: control.length } };
      }
      return null;
    };
  }
  

  getIngredientName(ingredientId: number): string {
    const ingredient = this.availableIngredients.find(
      (i) => i.id === ingredientId
    );
    return ingredient ? ingredient.name : "";
  }

  getMaxAvailableQuantity(ingredientId: number): number {
    const ingredient = this.availableIngredients.find(
      (i) => i.id === ingredientId
    );
    return ingredient ? ingredient.quantity : 0;
  }

  protected markFormGroupTouched(formGroup: FormGroup | FormArray): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup || control instanceof FormArray) {
        this.markFormGroupTouched(control);
      }
    });
  }  

  private validateArrayItems(array: FormArray): boolean {
    return array.controls.every(control => control.valid);
  }
  

}
