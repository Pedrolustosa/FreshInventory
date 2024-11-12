import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IngredientService } from '../../../services/ingredient.service';
import { Unit } from '../../../models/enums/unit.enum';
import { Category } from '../../../models/enums/category.enum';

export abstract class IngredientFormBase {
  ingredientForm: FormGroup;
  units = Object.values(Unit);
  categories = Object.values(Category);

  constructor(
    protected ingredientService: IngredientService,
    protected router: Router
  ) {
    this.ingredientForm = new FormBuilder().group({
      name: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(0)]],
      unit: ['', Validators.required],
      unitCost: ['', [Validators.required, Validators.min(0)]],
      category: ['', Validators.required],
      supplier: ['', Validators.required],
      purchaseDate: ['', Validators.required],
      expiryDate: ['', Validators.required],
      isPerishable: [false],
      reorderLevel: ['', [Validators.required, Validators.min(0)]]
    });
  }

  abstract onSubmit(): void;

  protected formatDate(date: string | Date): string {
    return new Date(date).toISOString().split('T')[0];
  }

  protected markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}