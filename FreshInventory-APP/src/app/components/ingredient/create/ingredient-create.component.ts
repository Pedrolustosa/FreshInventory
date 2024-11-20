import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { Router } from '@angular/router';
import { IngredientService } from '../../../services/ingredient.service';
import { SupplierService } from '../../../services/supplier.service';
import { IngredientFormBase } from '../shared/ingredient-form.base';
import { Unit } from '../../../models/enums/unit.enum';
import { Category } from '../../../models/enums/category.enum';

@Component({
  selector: 'app-ingredient-create',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule
  ],
  templateUrl: './ingredient-create.component.html',
  styleUrls: ['./ingredient-create.component.css']
})
export class IngredientCreateComponent extends IngredientFormBase {
  protected readonly Unit = Unit;
  protected readonly Category = Category;

  constructor(
    protected override ingredientService: IngredientService,
    protected override supplierService: SupplierService,
    protected override router: Router
  ) {
    super(ingredientService, supplierService, router);
  }

  override onSubmit(): void {
    if (this.ingredientForm.valid) {
      this.ingredientService.createIngredient(this.ingredientForm.value).subscribe({
        next: () => {
          this.router.navigate(['/ingredients']);
        },
        error: () => {
          // Handle error
        }
      });
    } else {
      this.markFormGroupTouched(this.ingredientForm);
    }
  }
}