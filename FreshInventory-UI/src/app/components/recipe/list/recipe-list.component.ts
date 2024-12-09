import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { RecipeService } from "src/app/services/recipe.service";
import { IngredientService } from "src/app/services/ingredient.service";
import { RecipeReadDto } from "src/app/models/recipe.model";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { TooltipModule } from "ngx-bootstrap/tooltip";
import { FormsModule } from "@angular/forms";
import { Modal } from "bootstrap";
import { finalize } from "rxjs";
import jsPDF from "jspdf";

@Component({
  selector: "app-recipe-list",
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    PaginationModule,
    TooltipModule,
    FormsModule,
    NgxSpinnerModule,
  ],
  templateUrl: "./recipe-list.component.html",
  styleUrls: ["./recipe-list.component.scss"],
})
export class RecipeListComponent implements OnInit {
  // Data
  recipes: RecipeReadDto[] = [];
  selectedRecipe: RecipeReadDto | null = null;
  ingredientNames: { [key: number]: string } = {};
  Math = Math;

  // Pagination
  currentPage: number = 1;
  itemsPerPage: number = 10;
  totalItems: number = 0;
  maxSize: number = 5;

  // Search
  searchName: string = "";

  constructor(
    private recipeService: RecipeService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private ingredientService: IngredientService
  ) {}

  ngOnInit(): void {
    this.loadRecipes();
  }

  // Recipe Operations
  loadRecipes(): void {
    this.spinner.show();
    this.recipeService
      .getAllRecipesPaged(this.currentPage, this.itemsPerPage)
      .subscribe({
        next: (response) => {
          this.recipes = response.data || [];
          this.totalItems = response.totalCount || 0;
        },
        error: (error) => {
          console.error("Error loading recipes:", error);
          this.toastr.error("Failed to load recipes");
        },
        complete: () => this.spinner.hide(),
      });
  }

  showRecipeDetails(recipe: RecipeReadDto): void {
    this.selectedRecipe = recipe;
    const modalElement = document.getElementById("recipeDetailsModal");
    if (modalElement) {
      Modal.getOrCreateInstance(modalElement).show();
    }
  }

  deleteRecipe(): void {
    if (!this.selectedRecipe) return;

    this.spinner.show();
    this.recipeService
      .deleteRecipe(this.selectedRecipe.id)
      .pipe(finalize(() => this.spinner.hide()))
      .subscribe({
        next: () => {
          this.toastr.success("Recipe deleted successfully");
          this.loadRecipes();
          this.selectedRecipe = null;
        },
        error: () => {
          this.toastr.error("Failed to delete recipe. Please try again.");
        },
      });
  }

  // PDF Download
  downloadRecipeAsPDF(): void {
    if (!this.selectedRecipe) return;

    const doc = new jsPDF("portrait", "mm", "a4");
    const pageWidth = doc.internal.pageSize.getWidth();

    // Header
    doc.setFillColor(240, 240, 240); // Light gray background
    doc.rect(0, 0, pageWidth, 40, "F"); // Header background
    doc.setFontSize(22);
    doc.setFont("helvetica", "bold");
    doc.setTextColor(40, 40, 40);
    doc.text(this.selectedRecipe.name, pageWidth / 2, 20, { align: "center" });

    doc.setFontSize(12);
    doc.setFont("helvetica", "italic");
    doc.setTextColor(80, 80, 80);
    doc.text(this.selectedRecipe.description, pageWidth / 2, 30, {
      align: "center",
    });

    // Details Section
    doc.setFontSize(16);
    doc.setFont("helvetica", "bold");
    doc.setTextColor(0, 0, 0);
    doc.text("Details", 10, 50);

    doc.setFontSize(12);
    doc.setFont("helvetica", "normal");
    doc.text(`Servings: ${this.selectedRecipe.servings}`, 10, 60);
    doc.text(
      `Preparation Time: ${this.selectedRecipe.preparationTime}`,
      10,
      70
    );

    // Divider
    doc.setDrawColor(200, 200, 200);
    doc.line(10, 75, pageWidth - 10, 75); // Horizontal line

    // Ingredients Section
    doc.setFontSize(16);
    doc.setFont("helvetica", "bold");
    doc.text("Ingredients", 10, 85);

    doc.setFontSize(12);
    doc.setFont("helvetica", "normal");
    this.selectedRecipe.ingredients.forEach((ingredient, index) => {
      doc.text(
        `${index + 1}. ${ingredient.ingredientName} - ${ingredient.quantity}`,
        10,
        95 + index * 8
      );
    });

    const ingredientsEndY = 95 + this.selectedRecipe.ingredients.length * 8;

    // Divider
    doc.line(10, ingredientsEndY + 5, pageWidth - 10, ingredientsEndY + 5);

    // Steps Section
    doc.setFontSize(16);
    doc.setFont("helvetica", "bold");
    doc.text("Steps", 10, ingredientsEndY + 15);

    doc.setFontSize(12);
    doc.setFont("helvetica", "normal");
    this.selectedRecipe.steps.forEach((step, index) => {
      const yPosition = ingredientsEndY + 25 + index * 10;

      if (yPosition > doc.internal.pageSize.height - 20) {
        doc.addPage();
        doc.setFontSize(16);
        doc.setFont("helvetica", "bold");
        doc.text("Steps (continued)", 10, 20);
        doc.setFontSize(12);
        doc.setFont("helvetica", "normal");
      }

      doc.text(`${index + 1}. ${step}`, 10, yPosition);
    });

    // Footer
    const currentDate = new Date().toLocaleDateString();
    doc.setFontSize(10);
    doc.setFont("helvetica", "italic");
    doc.setTextColor(120, 120, 120);
    doc.text(
      `Generated on ${currentDate} - FreshInventory Recipe System`,
      pageWidth / 2,
      doc.internal.pageSize.height - 10,
      { align: "center" }
    );

    // Save PDF
    doc.save(`${this.selectedRecipe.name}_Recipe.pdf`);
  }

  // Pagination
  pageChanged(event: any): void {
    this.currentPage = event.page;
    this.loadRecipes();
  }

  // Search
  onSearch(): void {
    this.currentPage = 1;
    this.loadRecipes();
  }

  // Modal Operations
  openDeleteModal(recipe: RecipeReadDto): void {
    this.selectedRecipe = recipe;
    const modalElement = document.getElementById("deleteModal");
    if (modalElement) {
      Modal.getOrCreateInstance(modalElement).show();
    }
  }
}
