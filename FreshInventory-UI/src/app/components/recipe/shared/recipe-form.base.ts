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
import { CreateRecipe } from "../../../models/recipe.model";

export abstract class RecipeFormBase {
  recipeForm!: FormGroup;
  availableIngredients: Ingredient[] = [];
  categories = ["Main Course", "Appetizer", "Dessert", "Beverage", "Side Dish"];
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
    // Create form arrays first
    const ingredientsArray = this.fb.array([]);
    const instructionsArray = this.fb.array([]);

    // Create the form
    this.recipeForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      category: ['', Validators.required],
      preparationTime: ['', [Validators.required, Validators.min(1)]],
      servings: ['', [Validators.required, Validators.min(1)]], 
      isAvailable: [true], // Adding isAvailable field with default value true
      ingredients: ingredientsArray,
      instructions: instructionsArray 
    });

    // Add validators after form is created
    ingredientsArray.setValidators(this.ingredientsValidator());
    instructionsArray.setValidators(this.instructionsValidator());
  }

  private ingredientsValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control instanceof FormArray) {        
        const hasValidIngredient = control.controls.some(ctrl => {
          if (ctrl instanceof FormGroup) {
            const ingredientId = ctrl.get('ingredientId')?.value;
            const quantity = ctrl.get('quantity')?.value;
            return Boolean(ingredientId && quantity && Number(quantity) > 0);
          }
          return false;
        });

        return hasValidIngredient ? null : { required: true };
      }
      return null;
    };
  }

  private instructionsValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control instanceof FormArray) {
        const hasValidInstruction = control.controls.some(ctrl => 
          ctrl.value && typeof ctrl.value === 'string' && ctrl.value.trim().length > 0
        );
        return hasValidInstruction ? null : { minArrayLength: { requiredLength: 1, actualLength: 0 } };
      }
      return null;
    };
  }

  get ingredientsArray(): FormArray {
    return this.recipeForm.get('ingredients') as FormArray;
  }

  get instructionsArray(): FormArray {
    return this.recipeForm.get('instructions') as FormArray;
  }

  addIngredient(): void {
    const ingredientGroup = this.fb.group({
      ingredientId: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(0.1)]],
    });

    this.ingredientsArray.push(ingredientGroup);

    ingredientGroup.valueChanges.subscribe(() => {
      const ingredientId = ingredientGroup.get('ingredientId')?.value;
      const quantity = ingredientGroup.get('quantity')?.value;
      
      console.log('Valores alterados:', {
        ingredientId,
        quantity,
        ingredientIdType: typeof ingredientId,
        quantityType: typeof quantity,
        maxQuantity: this.getMaxAvailableQuantity(ingredientId)
      });
      
      if (ingredientId && quantity && Number(quantity) > 0) {
        console.log('Limpando erros do grupo');
        ingredientGroup.setErrors(null);
        this.ingredientsArray.setErrors(null);
        this.ingredientsArray.updateValueAndValidity({ emitEvent: false });
      }
    });
  }

  removeIngredient(index: number): void {
    this.ingredientsArray.removeAt(index);
    this.ingredientsArray.markAsTouched();
    this.ingredientsArray.updateValueAndValidity();
  }

  addInstruction(): void {
    const instructionControl = this.fb.control('', Validators.required);

    instructionControl.valueChanges.subscribe(() => {
      if (instructionControl.value && instructionControl.value.trim()) {
        instructionControl.setErrors(null);
      }
      
      this.instructionsArray.markAsTouched();
      this.instructionsArray.updateValueAndValidity();
    });

    this.instructionsArray.push(instructionControl);
    this.instructionsArray.updateValueAndValidity();
  }

  removeInstruction(index: number): void {
    this.instructionsArray.removeAt(index);
    this.instructionsArray.markAsTouched();
    this.instructionsArray.updateValueAndValidity();
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

  protected getFormattedRecipe(): CreateRecipe | null {
    if (this.recipeForm.valid) {
      const formValue = this.recipeForm.value;
      console.log('Form value antes da formatação:', formValue);
      
      // Ensure ingredients are properly formatted
      const ingredients = formValue.ingredients
        .filter((ing: any) => {
          const isValid = ing.ingredientId && ing.quantity && Number(ing.quantity) > 0;
          console.log('Validando ingrediente na formatação:', {
            ingrediente: ing,
            isValid
          });
          return isValid;
        })
        .map((ing: any) => ({
          ingredientId: Number(ing.ingredientId),
          quantity: Number(ing.quantity)
        }));

      // Ensure instructions are properly formatted
      const instructions = formValue.instructions
        .filter((inst: string) => inst && inst.trim())
        .map((inst: string) => inst.trim());

      const recipe: CreateRecipe = {
        name: formValue.name?.trim(),
        category: formValue.category?.trim(),
        description: formValue.description?.trim(),
        preparationTime: Number(formValue.preparationTime),
        servings: Number(formValue.servings),
        isAvailable: formValue.isAvailable,
        ingredients,
        instructions
      };

      console.log('Recipe formatada:', recipe);
      return recipe;
    }

    console.log('Form inválido:', this.recipeForm.errors);
    this.markFormGroupTouched(this.recipeForm);
    return null;
  }

  protected markFormGroupTouched(formGroup: FormGroup | FormArray): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      } else if (control instanceof FormArray) {
        this.markFormGroupTouched(control);
      }

      if (control.errors) {
        console.log('Erros no controle:', control.errors);
      }
    });
  }

  abstract onSubmit(): void;

  getIngredientName(ingredientId: number): string {
    const ingredient = this.availableIngredients.find(
      (i) => i.id === ingredientId
    );
    return ingredient ? ingredient.name : "";
  }

  getMaxAvailableQuantity(ingredientId: string | number | null | undefined): number {
    if (!ingredientId) return 0;
    
    const ingredient = this.availableIngredients.find(
      (i) => i.id === Number(ingredientId)
    );
    return ingredient ? ingredient.quantity : 0;
  }

  getControlError(controlName: string): string {
    const control = this.recipeForm.get(controlName);
    if (control && control.errors && control.touched) {
      if (control.errors['required']) {
        return 'This field is required';
      }
      if (control.errors['min']) {
        return `Value must be at least ${control.errors['min'].min}`;
      }
      if (control.errors['minArrayLength']) {
        return `At least ${control.errors['minArrayLength'].requiredLength} item(s) required`;
      }
    }
    return '';
  }
}
