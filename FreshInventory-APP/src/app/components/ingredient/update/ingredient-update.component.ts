import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { IngredientService } from '../../../services/ingredient.service';
import { SupplierService } from '../../../services/supplier.service';
import { IngredientFormBase } from '../shared/ingredient-form.base';

@Component({
  selector: 'app-ingredient-update',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule
  ],
  templateUrl: './ingredient-update.component.html',
  styleUrls: ['./ingredient-update.component.css']
})
export class IngredientUpdateComponent extends IngredientFormBase implements OnInit {
  ingredientId: number = 0;

  constructor(
    protected override ingredientService: IngredientService,
    protected override supplierService: SupplierService,
    protected override router: Router,
    private route: ActivatedRoute
  ) {
    super(ingredientService, supplierService, router);
  }

  ngOnInit(): void {
    this.ingredientId = this.route.snapshot.params['id'];
    this.loadIngredient();
  }

  private loadIngredient(): void {
    this.ingredientService.getIngredientById(this.ingredientId).subscribe({
      next: (ingredient) => {
        this.ingredientForm.patchValue({
          ...ingredient,
          purchaseDate: this.formatDate(ingredient.purchaseDate),
          expiryDate: this.formatDate(ingredient.expiryDate)
        });
      },
      error: () => {
        this.router.navigate(['/ingredients']);
      }
    });
  }

  override onSubmit(): void {
    if (this.ingredientForm.valid) {
      this.ingredientService.updateIngredient(this.ingredientId, this.ingredientForm.value).subscribe({
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