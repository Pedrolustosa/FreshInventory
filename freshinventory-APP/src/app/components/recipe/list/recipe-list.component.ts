import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { RecipeService } from '../../../services/recipe.service';
import { Recipe } from '../../../models/recipe.model';
import { RecipeDeleteModalComponent } from '../modals/delete-confirmation-modal.component';

@Component({
  selector: 'app-recipe-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    NgxSpinnerModule,
    RecipeDeleteModalComponent
  ],
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.css']
})
export class RecipeListComponent implements OnInit {
  recipes: Recipe[] = [];
  filteredRecipes: Recipe[] = [];
  searchTerm = '';
  selectedCategory = '';
  showOnlyAvailable = false;
  showDeleteModal = false;
  selectedRecipe: Recipe | null = null;

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
    this.recipeService.getRecipes().subscribe({
      next: (recipes) => {
        this.recipes = recipes;
        this.filterRecipes();
      },
      error: () => {
        this.toastr.error('Error loading recipes');
      }
    });
  }

  filterRecipes(): void {
    this.filteredRecipes = this.recipes.filter(recipe => {
      const matchesSearch = recipe.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
                          recipe.description.toLowerCase().includes(this.searchTerm.toLowerCase());
      const matchesCategory = !this.selectedCategory || recipe.category === this.selectedCategory;
      const matchesAvailability = !this.showOnlyAvailable || recipe.isAvailable;
      
      return matchesSearch && matchesCategory && matchesAvailability;
    });
  }

  onSearch(): void {
    this.filterRecipes();
  }

  onCategoryChange(): void {
    this.filterRecipes();
  }

  onAvailabilityChange(): void {
    this.filterRecipes();
  }

  getAvailabilityClass(recipe: Recipe): string {
    return recipe.isAvailable ? 'available' : 'unavailable';
  }

  onDeleteClick(recipe: Recipe): void {
    this.selectedRecipe = recipe;
    this.showDeleteModal = true;
  }

  onDeleteConfirm(): void {
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

  closeDeleteModal(): void {
    this.showDeleteModal = false;
    this.selectedRecipe = null;
  }
}