import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { RecipeService } from 'src/app/services/recipe.service';
import { IngredientService } from 'src/app/services/ingredient.service';
import { RecipeReadDto } from 'src/app/models/recipe.model';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { FormsModule } from '@angular/forms';
import { Modal } from 'bootstrap';

@Component({
  selector: 'app-recipe-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    PaginationModule,
    TooltipModule,
    FormsModule,
    NgxSpinnerModule
  ],
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.scss']
})
export class RecipeListComponent implements OnInit {
  recipes: RecipeReadDto[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 10;
  totalItems: number = 0;
  maxSize: number = 5;
  selectedRecipe: RecipeReadDto | null = null;
  searchName: string = '';
  ingredientNames: { [key: number]: string } = {};
  private deleteModal?: Modal;
  private detailsModal?: Modal;
  Math = Math;

  constructor(
    private recipeService: RecipeService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private ingredientService: IngredientService
  ) {}

  ngOnInit(): void {
    this.loadRecipes();
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadRecipes();
  }

  loadRecipes(): void {
    this.spinner.show();
    this.recipeService
      .getAllRecipesPaged(this.currentPage, this.itemsPerPage)
      .subscribe({
        next: (response) => {
          console.log("response.data:", response.data);
          console.log("response:", response?.data);
          console.log("response:", response?.totalCount);
          this.recipes = response.data || [];
          this.totalItems = response.totalCount || 0;
        },
        error: (error) => {
          console.error('Error loading recipes:', error);
          this.toastr.error('Failed to load recipes');
        },
        complete: () => {
          this.spinner.hide();
        },
      });
  }

  pageChanged(event: any): void {
    this.currentPage = event.page;
    this.loadRecipes();
  }

  openDeleteModal(recipe: RecipeReadDto): void {
    this.selectedRecipe = recipe;
    const modalElement = document.getElementById('deleteModal');
    if (modalElement) {
      this.deleteModal = new Modal(modalElement);
      this.deleteModal.show();
    }
  }

  confirmDelete(): void {
    if (this.selectedRecipe) {
      this.spinner.show();
      this.recipeService.deleteRecipe(this.selectedRecipe.id).subscribe({
        next: () => {
          this.toastr.success('Recipe deleted successfully');
          this.loadRecipes();
        },
        error: (error) => {
          console.error('Error deleting recipe:', error);
          this.toastr.error('Failed to delete recipe');
        },
        complete: () => {
          this.spinner.hide();
          this.closeDeleteModal();
        },
      });
    }
  }

  closeDeleteModal(): void {
    this.deleteModal?.hide();
    this.selectedRecipe = null;
  }

  showRecipeDetails(recipe: RecipeReadDto): void {
    this.selectedRecipe = recipe;
    if (!this.detailsModal) {
      const modalElement = document.getElementById('recipeDetailsModal');
      if (modalElement) {
        this.detailsModal = new Modal(modalElement);
      }
    }
    this.detailsModal?.show();
  }
}
