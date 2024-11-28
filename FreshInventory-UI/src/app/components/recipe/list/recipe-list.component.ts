import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { RecipeService } from 'src/app/services/recipe.service';
import { IngredientService } from 'src/app/services/ingredient.service';
import { Recipe } from 'src/app/models/recipe.model';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { FormsModule } from '@angular/forms';
import { Modal } from 'bootstrap';
import jsPDF from 'jspdf';

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
  private detailsModal?: Modal;
  ingredientNames: { [key: number]: string } = {};

  constructor(
    private recipeService: RecipeService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private ingredientService: IngredientService
  ) {}

  ngOnInit(): void {
    this.loadRecipes();
    this.loadIngredients();
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

  private loadIngredients(): void {
    this.ingredientService.getIngredients(1, 100).subscribe({
      next: (response) => {
        response.items.forEach((ingredient: any) => {
          this.ingredientNames[ingredient.id] = ingredient.name;
        });
      },
      error: () => {
        console.error('Error loading ingredients');
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

  getIngredientName(ingredientId: number): string {
    return this.ingredientNames[ingredientId] || `Ingredient ${ingredientId}`;
  }

  showRecipeDetails(recipe: Recipe): void {
    this.selectedRecipe = recipe;
    if (!this.detailsModal) {
      const modalElement = document.getElementById('recipeDetailsModal');
      if (modalElement) {
        this.detailsModal = new Modal(modalElement);
      }
    }
    this.detailsModal?.show();
  }

  formatIngredientQuantity(quantity: number): string {
    return quantity % 1 === 0 ? quantity.toString() : quantity.toFixed(2);
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

  generatePDF(): void {
    if (!this.selectedRecipe) return;

    const doc = new jsPDF();
    const pageWidth = doc.internal.pageSize.width;
    let yPos = 20;
    const lineHeight = 7;
    const margin = 20;

    // Title
    doc.setFontSize(20);
    doc.setFont('helvetica', 'bold');
    doc.text(this.selectedRecipe.name, pageWidth / 2, yPos, { align: 'center' });
    yPos += lineHeight * 2;

    // Meta information
    doc.setFontSize(12);
    doc.setFont('helvetica', 'normal');
    const metaInfo = [
      `Category: ${this.selectedRecipe.category}`,
      `Preparation Time: ${this.formatTime(this.selectedRecipe.preparationTime)}`,
      `Servings: ${this.getServingsLabel(this.selectedRecipe.servings)}`,
      `Status: ${this.selectedRecipe.isAvailable ? 'Available' : 'Not Available'}`
    ];

    metaInfo.forEach(info => {
      doc.text(info, margin, yPos);
      yPos += lineHeight;
    });
    yPos += lineHeight;

    // Description
    doc.setFont('helvetica', 'bold');
    doc.text('Description:', margin, yPos);
    yPos += lineHeight;
    doc.setFont('helvetica', 'normal');
    const descriptionLines = doc.splitTextToSize(this.selectedRecipe.description, pageWidth - margin * 2);
    doc.text(descriptionLines, margin, yPos);
    yPos += lineHeight * (descriptionLines.length + 1);

    // Ingredients
    doc.setFont('helvetica', 'bold');
    doc.text('Ingredients:', margin, yPos);
    yPos += lineHeight;
    doc.setFont('helvetica', 'normal');

    this.selectedRecipe.ingredients.forEach(ingredient => {
      const ingredientText = `• ${this.getIngredientName(ingredient.ingredientId)}: ${this.formatIngredientQuantity(ingredient.quantity)}`;
      doc.text(ingredientText, margin, yPos);
      yPos += lineHeight;
    });
    yPos += lineHeight;

    // Instructions
    doc.setFont('helvetica', 'bold');
    doc.text('Instructions:', margin, yPos);
    yPos += lineHeight;
    doc.setFont('helvetica', 'normal');

    this.selectedRecipe.instructions.forEach((instruction, index) => {
      const stepText = `${index + 1}. ${instruction}`;
      const lines = doc.splitTextToSize(stepText, pageWidth - margin * 2);
      doc.text(lines, margin, yPos);
      yPos += lineHeight * lines.length;

      // Add some spacing between instructions
      yPos += lineHeight / 2;

      // Check if we need a new page
      if (yPos > doc.internal.pageSize.height - margin) {
        doc.addPage();
        yPos = margin;
      }
    });

    // Save the PDF
    const fileName = `${this.selectedRecipe.name.replace(/\s+/g, '_')}_recipe.pdf`;
    doc.save(fileName);
    this.toastr.success('PDF generated successfully', 'Success');
  }
}