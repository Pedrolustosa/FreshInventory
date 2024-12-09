import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
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
    NgxSpinnerModule,
  ],
  templateUrl: './recipe-create.component.html',
  styleUrls: ['./recipe-create.component.css'],
})
export class RecipeCreateComponent extends RecipeFormBase implements OnInit {
  constructor(
    protected override recipeService: RecipeService,
    protected override ingredientService: IngredientService,
    protected override router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    super(recipeService, ingredientService, router);
  }

  ngOnInit(): void {
    this.addIngredient();
    this.addStep();
  }

  override onSubmit(): void {
    if (this.recipeForm.valid) {
      const recipe = this.getFormattedRecipe();
      if (recipe) {
        this.spinner.show();
        this.recipeService.createRecipe(recipe).subscribe({
          next: () => {
            this.spinner.hide();
            this.toastr.success('Recipe created successfully!', 'Success');
            this.router.navigate(['/recipes']);
          },
          error: (error) => {
            this.spinner.hide();
            console.error('Error creating recipe:', error);
            this.toastr.error(
              'Failed to create recipe. Please try again.',
              'Error'
            );
          },
        });
      } else {
        this.toastr.error('Please review your inputs.', 'Validation Error');
      }
    } else {
      this.markFormGroupTouched(this.recipeForm);
      this.toastr.warning(
        'Please fill in all required fields.',
        'Validation Error'
      );
    }
  }
}
