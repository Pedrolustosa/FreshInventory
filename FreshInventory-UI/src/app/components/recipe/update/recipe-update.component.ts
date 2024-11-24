import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormArray } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { RecipeService } from '../../../services/recipe.service';
import { IngredientService } from '../../../services/ingredient.service';
import { RecipeFormBase } from '../shared/recipe-form.base';

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
    private toastr: ToastrService
  ) {
    super(recipeService, ingredientService, router);
  }

  ngOnInit(): void {
    this.recipeId = Number(this.route.snapshot.params['id']);
    this.loadRecipe();
  }

  private loadRecipe(): void {
    this.recipeService.getRecipeById(this.recipeId).subscribe({
      next: (recipe) => {
        this.recipeForm.patchValue({
          name: recipe.name,
          description: recipe.description,
          category: recipe.category,
          preparationTime: recipe.preparationTime,
          servings: recipe.servings
        });

        // Clear and rebuild ingredients array
        while (this.ingredientsArray.length) {
          this.ingredientsArray.removeAt(0);
        }
        recipe.ingredients.forEach(ingredient => {
          this.addIngredient();
          this.ingredientsArray.at(this.ingredientsArray.length - 1).patchValue(ingredient);
        });

        // Clear and rebuild instructions array
        while (this.instructionsArray.length) {
          this.instructionsArray.removeAt(0);
        }
        recipe.instructions.forEach(instruction => {
          this.addInstruction();
          this.instructionsArray.at(this.instructionsArray.length - 1).patchValue(instruction);
        });
      },
      error: () => {
        this.toastr.error('Error loading recipe');
        this.router.navigate(['/recipes']);
      }
    });
  }

  onSubmit(): void {
    if (this.recipeForm.valid) {
      this.recipeService.updateRecipe(this.recipeId, this.recipeForm.value).subscribe({
        next: () => {
          this.toastr.success('Recipe updated successfully');
          this.router.navigate(['/recipes']);
        },
        error: () => {
          this.toastr.error('Error updating recipe');
        }
      });
    } else {
      this.markFormGroupTouched(this.recipeForm);
      this.toastr.warning('Please fill in all required fields');
    }
  }
}