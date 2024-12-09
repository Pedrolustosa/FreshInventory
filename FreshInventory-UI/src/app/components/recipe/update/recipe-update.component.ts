import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { RecipeFormBase } from '../shared/recipe-form.base';
import { RecipeUpdateDto } from '../../../models/recipe.model';

@Component({
  selector: 'app-recipe-update',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule
  ],
  templateUrl: './recipe-update.component.html',
  styleUrls: ['./recipe-update.component.css']
})
export class RecipeUpdateComponent extends RecipeFormBase implements OnInit {
  recipeId: number = 0;

  constructor(
    protected override recipeService: RecipeService,
    protected override ingredientService: IngredientService,
    protected override router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    super(recipeService, ingredientService, router);
  }

  ngOnInit(): void {
    this.recipeId = Number(this.route.snapshot.params['id']);
    this.loadRecipe();
  }

  private loadRecipe(): void {
    this.spinner.show();
    this.recipeService.getRecipeById(this.recipeId).subscribe({
      next: (recipe) => {
        this.recipeForm.patchValue({
          name: recipe.name,
          description: recipe.description,
          servings: recipe.servings,
          preparationTime: recipe.preparationTime
        });

        this.ingredientsArray.clear();
        recipe.ingredients.forEach((ingredient) => {
          this.ingredientsArray.push(
            this.createIngredientGroup({
              ingredientId: ingredient.ingredientId,
              quantity: ingredient.quantity,
            })
          );
        });

        this.stepsArray.clear();
        recipe.steps.forEach((step) => {
          this.stepsArray.push(this.createStepControl(step));
        });

        this.spinner.hide();
      },
      error: () => {
        this.spinner.hide();
        this.toastr.error('Error loading recipe');
        this.router.navigate(['/recipes']);
      },
    });
  }

  override onSubmit(): void {
    if (this.recipeForm.valid) {
      const updatedRecipe: RecipeUpdateDto | null = this.getFormattedRecipe();
      if (updatedRecipe) {
        this.spinner.show();
        this.recipeService.updateRecipe(this.recipeId, updatedRecipe).subscribe({
          next: () => {
            this.spinner.hide();
            this.toastr.success('Recipe updated successfully!');
            this.router.navigate(['/recipes']);
          },
          error: () => {
            this.spinner.hide();
            this.toastr.error('Error updating recipe.');
          },
        });
      }
    } else {
      this.markFormGroupTouched(this.recipeForm);
      this.toastr.warning('Please fill in all required fields');
    }
  }
}
