import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { Router } from '@angular/router';
import { IngredientService } from '../../../services/ingredient.service';
import { IngredientFormBase } from '../shared/ingredient-form.base';

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
  constructor(
    protected override ingredientService: IngredientService,
    protected override router: Router
  ) {
    super(ingredientService, router);
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