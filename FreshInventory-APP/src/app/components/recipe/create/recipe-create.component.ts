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
    NgxSpinnerModule
  ],
  templateUrl: './recipe-create.component.html',
  styleUrls: ['./recipe-create.component.css']
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
    this.addInstruction();
  }

  onSubmit(): void {
    if (this.recipeForm.valid) {
      this.spinner.show();
      this.recipeService.createRecipe(this.recipeForm.value).subscribe({
        next: () => {
          this.spinner.hide();
          this.toastr.success('Recipe created successfully', 'Success');
          this.router.navigate(['/recipes']);
        },
        error: () => {
          this.spinner.hide();
          this.toastr.error('Error creating recipe', 'Error');
        }
      });
    } else {
      this.markFormGroupTouched(this.recipeForm);
      this.toastr.warning('Please fill in all required fields', 'Warning');
    }
  }  
}
