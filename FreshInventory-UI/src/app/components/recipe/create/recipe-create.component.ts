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

  override onSubmit(): void {
    console.log('Iniciando submissão do formulário');
    console.log('Form válido:', this.recipeForm.valid);
    console.log('Form value:', this.recipeForm.value);
    console.log('Form errors:', this.recipeForm.errors);

    if (this.recipeForm.valid) {
      const recipe = this.getFormattedRecipe();
      if (recipe !== null) {
        this.spinner.show();
        this.recipeService.createRecipe(recipe).subscribe({
          next: () => {
            this.spinner.hide();
            this.toastr.success('Recipe created successfully', 'Success');
            this.router.navigate(['/recipes']);
          },
          error: (error) => {
            this.spinner.hide();
            console.error('Erro ao criar receita:', error);
            this.toastr.error('Error creating recipe', 'Error');
          }
        });
      } else {
        console.error('Recipe is null after formatting');
        this.toastr.error('Please fill in all required fields', 'Validation Error');
      }
    } else {
      console.log('Form inválido - marcando campos como touched');
      this.markFormGroupTouched(this.recipeForm);
      this.toastr.error('Please fill in all required fields', 'Validation Error');
    }
  }  
}
