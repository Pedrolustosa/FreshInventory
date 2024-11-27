import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { RecipeService } from 'src/app/services/recipe.service';
import { Recipe } from 'src/app/models/recipe.model';
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
  recipes: Recipe[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 10;
  totalItems: number = 0;
  maxSize: number = 5;
  selectedRecipe: Recipe | null = null;
  searchName: string = '';
  protected readonly Math = Math;
  private deleteModal?: Modal;

  constructor(
    private recipeService: RecipeService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
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
    this.recipeService.getRecipes(this.currentPage, this.itemsPerPage, this.searchName)
      .subscribe({
        next: (response) => {
          this.recipes = response.items || [];
          this.totalItems = response.totalCount || 0;
          console.debug('Loaded recipes:', this.recipes);
        },
        error: (error) => {
          console.error('Error loading recipes:', error);
          this.toastr.error('Failed to load recipes');
        },
        complete: () => {
          this.spinner.hide();
        }
      });
  }

  pageChanged(event: any): void {
    this.currentPage = event.page;
    this.loadRecipes();
  }

  getCategoryBadgeClass(category: string): string {
    const categoryMap: { [key: string]: string } = {
      'Main Course': 'primary',
      'Appetizer': 'success',
      'Dessert': 'warning',
      'Beverage': 'info',
      'Side Dish': 'secondary',
      'Breakfast': 'danger',
      'Snack': 'dark'
    };
    return categoryMap[category] || 'primary';
  }

  formatTime(minutes: number): string {
    if (minutes < 60) {
      return `${minutes} min`;
    }
    const hours = Math.floor(minutes / 60);
    const remainingMinutes = minutes % 60;
    return remainingMinutes > 0 
      ? `${hours}h ${remainingMinutes}m`
      : `${hours}h`;
  }

  getServingsLabel(servings: number): string {
    return `${servings} ${servings === 1 ? 'serving' : 'servings'}`;
  }

  getIngredientsLabel(count: number): string {
    return `${count} ${count === 1 ? 'ingredient' : 'ingredients'}`;
  }

  getInstructionsLabel(count: number): string {
    return `${count} ${count === 1 ? 'step' : 'steps'}`;
  }

  openDeleteModal(recipe: Recipe): void {
    this.selectedRecipe = recipe;
    const modalElement = document.getElementById('deleteModal');
    if (modalElement) {
      this.deleteModal = new Modal(modalElement);
      this.deleteModal.show();
    }
  }

  closeDeleteModal(): void {
    this.deleteModal?.hide();
    this.selectedRecipe = null;
  }

  confirmDelete(): void {
    if (this.selectedRecipe) {
      this.spinner.show();
      this.recipeService.deleteRecipe(this.selectedRecipe.id)
        .subscribe({
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
          }
        });
    }
  }
}