import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { RecipeService } from '../../../services/recipe.service';
import { Recipe } from '../../../models/recipe.model';
import { DeleteConfirmationModalComponent } from 'src/app/shared/delete-confirmation-modal/delete-confirmation-modal.component';

@Component({
  selector: 'app-recipe-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    NgxSpinnerModule,
    DeleteConfirmationModalComponent
  ],
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.css']
})
export class RecipeListComponent implements OnInit {
  recipes: Recipe[] = [];
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  searchTerm = '';
  selectedCategory = '';
  showOnlyAvailable = false;
  selectedRecipe: any = null;
  showDeleteModal: boolean = false;
  protected readonly Math = Math;

  categories = [
    'Main Course',
    'Appetizer',
    'Dessert',
    'Beverage',
    'Side Dish'
  ];

  constructor(
    private recipeService: RecipeService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadRecipes();
  }

  loadRecipes(): void {
    this.recipeService.getRecipes(
      this.currentPage,
      this.pageSize,
      this.searchTerm
    ).subscribe({
      next: (response) => {
        this.recipes = response.items;
        this.totalItems = response.totalCount;
        this.filterRecipes();
      },
      error: () => {
        this.toastr.error('Error loading recipes');
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadRecipes();
  }

  filterRecipes(): void {
    this.recipes = this.recipes.filter(recipe => {
      const matchesCategory = !this.selectedCategory || recipe.category === this.selectedCategory;
      return matchesCategory;
    });
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadRecipes();
  }

  onCategoryChange(): void {
    this.filterRecipes();
  }

  onAvailabilityChange(): void {
    this.filterRecipes();
  }

  onDeleteClick(recipe: Recipe): void {
    this.selectedRecipe = recipe;
    this.showDeleteModal = true;
  }

  confirmDelete(): void {
    if (this.selectedRecipe) {
      this.recipeService.deleteRecipe(this.selectedRecipe.id).subscribe({
        next: () => {
          this.toastr.success('Recipe deleted successfully');
          this.loadRecipes();
          this.closeDeleteModal();
        },
        error: () => {
          this.toastr.error('Error deleting recipe');
          this.closeDeleteModal();
        }
      });
    }
  }

  closeDeleteModal() {
    this.showDeleteModal = false;
  }
}