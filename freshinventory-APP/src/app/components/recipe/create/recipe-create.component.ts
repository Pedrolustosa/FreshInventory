import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { RecipeFormBase } from '../shared/recipe-form.base';

@Component({
  selector: 'app-recipe-create',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule
  ],
  templateUrl: './recipe-create.component.html',
  styleUrls: ['./recipe-create.component.css']
})
export class RecipeCreateComponent extends RecipeFormBase {
  constructor(
    protected override recipeService: RecipeService,
    protected override ingredientService: IngredientService,
    protected override router: Router,
    private toastr: ToastrService
  ) {
    super(recipeService, ingredientService, router);
    this.addIngredient();
    this.addInstruction();
  }

  override onSubmit(): void {
    if (this.recipeForm.valid) {
      this.recipeService.createRecipe(this.recipeForm.value).subscribe({
        next: () => {
          this.toastr.success('Recipe created successfully');
          this.router.navigate(['/recipes']);
        },
        error: () => {
          this.toastr.error('Error creating recipe');
        }
      });
    } else {
      this.markFormGroupTouched(this.recipeForm);
      this.toastr.warning('Please fill in all required fields');
    }
  }
}