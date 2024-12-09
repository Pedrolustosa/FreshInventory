import {
  FormBuilder,
  FormGroup,
  FormArray,
  Validators,
  AbstractControl,
  ValidatorFn,
  ValidationErrors,
} from '@angular/forms';
import { Router } from '@angular/router';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { RecipeCreateDto, RecipeIngredientDto } from '../../../models/recipe.model';
import { IngredientReadDto } from '../../../models/ingredient.model';

export abstract class RecipeFormBase {
  recipeForm!: FormGroup;
  availableIngredients: IngredientReadDto[] = [];
  private fb = new FormBuilder();

  constructor(
    protected recipeService: RecipeService,
    protected ingredientService: IngredientService,
    protected router: Router
  ) {
    this.initForm();
    this.loadAvailableIngredients();
  }

  private initForm(): void {
    this.recipeForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      preparationTime: ['', [Validators.required, Validators.min(1)]],
      servings: ['', [Validators.required, Validators.min(1)]],
      ingredients: this.fb.array([]),
      steps: this.fb.array([]),
    });

    this.ingredientsArray.setValidators(this.ingredientsValidator());
    this.stepsArray.setValidators(this.stepsValidator());
  }

  get ingredientsArray(): FormArray {
    return this.recipeForm.get('ingredients') as FormArray;
  }

  get stepsArray(): FormArray {
    return this.recipeForm.get('steps') as FormArray;
  }

  addIngredient(): void {
    this.ingredientsArray.push(
      this.fb.group({
        ingredientId: ['', Validators.required],
        quantity: ['', [Validators.required, Validators.min(0.1)]],
      })
    );
  }

  removeIngredient(index: number): void {
    this.ingredientsArray.removeAt(index);
  }

  addStep(): void {
    this.stepsArray.push(this.fb.control('', Validators.required));
  }

  removeStep(index: number): void {
    this.stepsArray.removeAt(index);
  }

  protected createIngredientGroup(data: Partial<{ ingredientId: number; quantity: number }> = {}) {
    return this.fb.group({
      ingredientId: [data.ingredientId || '', Validators.required],
      quantity: [data.quantity || '', [Validators.required, Validators.min(0.1)]],
    });
  }
  
  protected createStepControl(step: string = '') {
    return this.fb.control(step, Validators.required);
  }  

  private loadAvailableIngredients(): void {
    this.ingredientService
      .getAllIngredientsPaged(1, 100)
      .subscribe({
        next: (response) => {
          this.availableIngredients = response.data.filter(
            (ingredient) => ingredient.quantity > 0
          );
        },
        error: () => {
          console.error('Error loading ingredients.');
        },
      });
  }

  protected getFormattedRecipe(): RecipeCreateDto | null {
    if (this.recipeForm.valid) {
      const formValue = this.recipeForm.value;

      const ingredients: RecipeIngredientDto[] = formValue.ingredients.map(
        (ing: any) => ({
          ingredientId: Number(ing.ingredientId),
          quantity: Number(ing.quantity),
        })
      );

      const steps: string[] = formValue.steps.map((step: string) =>
        step.trim()
      );

      return {
        name: formValue.name.trim(),
        description: formValue.description.trim(),
        preparationTime: formValue.preparationTime,
        servings: formValue.servings,
        ingredients,
        steps,
      };
    }

    this.markFormGroupTouched(this.recipeForm);
    return null;
  }

  private ingredientsValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const hasValidIngredients = (control as FormArray).controls.some(
        (ctrl) =>
          ctrl.get('ingredientId')?.value &&
          ctrl.get('quantity')?.value > 0
      );

      return hasValidIngredients ? null : { required: true };
    };
  }

  private stepsValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const hasValidSteps = (control as FormArray).controls.some(
        (ctrl) => ctrl.value.trim().length > 0
      );

      return hasValidSteps ? null : { required: true };
    };
  }

  protected markFormGroupTouched(formGroup: FormGroup | FormArray): void {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      if (control instanceof FormGroup || control instanceof FormArray) {
        this.markFormGroupTouched(control);
      }
    });
  }

  abstract onSubmit(): void;

  getIngredientName(ingredientId: number): string {
    const ingredient = this.availableIngredients.find(
      (ing) => ing.id === ingredientId
    );
    return ingredient ? ingredient.name : '';
  }

  getMaxAvailableQuantity(ingredientId: number): number {
    const ingredient = this.availableIngredients.find(
      (ing) => ing.id === ingredientId
    );
    return ingredient ? ingredient.quantity : 0;
  }

  getControlError(controlName: string): string {
    const control = this.recipeForm.get(controlName);
    if (control?.errors && control.touched) {
      if (control.errors['required']) return 'This field is required.';
      if (control.errors['min'])
        return `Value must be at least ${control.errors['min'].min}.`;
    }
    return '';
  }
}
