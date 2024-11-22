import { Component, OnInit } from "@angular/core";
import { CommonModule, DatePipe } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { NgxSpinnerModule } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { IngredientService } from "../../../services/ingredient.service";
import { Ingredient } from "../../../models/ingredient.model";
import { Unit, UnitLabels } from "../../../models/enums/unit.enum";
import { Category, CategoryLabels } from "../../../models/enums/category.enum";
import { DeleteConfirmationModalComponent } from "../../../shared/delete-confirmation-modal/delete-confirmation-modal.component";

@Component({
  selector: "app-ingredient-list",
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    NgxSpinnerModule,
    DatePipe,
    DeleteConfirmationModalComponent
  ],
  templateUrl: "./ingredient-list.component.html",
  styleUrls: ["./ingredient-list.component.css"],
})
export class IngredientListComponent implements OnInit {
  ingredients: Ingredient[] = [];
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  searchName = "";
  selectedCategory: Category | "" = "";
  selectedIngredient: any = null;
  showDeleteModal: boolean = false;
  protected readonly Math = Math;
  protected readonly Category = Category;
  protected readonly Unit = Unit;
  categories = Object.values(Category).filter(
    (value) => typeof value === "number"
  ) as Category[];
  units = Object.values(Unit).filter(
    (value) => typeof value === "number"
  ) as Unit[];

  constructor(
    private ingredientService: IngredientService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadIngredients();
  }

  getUnitLabel(unit: number | Unit): string {
    return UnitLabels[unit as Unit] || "Unknown Unit";
  }

  getCategoryLabel(category: number | Category): string {
    return CategoryLabels[category as Category] || "Unknown Category";
  }

  loadIngredients(): void {
    this.ingredientService
      .getIngredients(
        this.currentPage,
        this.pageSize,
        this.searchName,
        this.selectedCategory ? this.selectedCategory.toString() : ""
      )
      .subscribe({
        next: (response) => {
          this.ingredients = response.items.map((item: any) => ({
            ...item,
            supplierName: item.supplierName || "Unknown Supplier",
          }));
          this.totalItems = response.totalCount;
        },
        error: (error) => {
          this.toastr.error("Error loading ingredients");
        },
      });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadIngredients();
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadIngredients();
  }

  openDeleteModal(ingredient: any) {
    this.selectedIngredient = ingredient;
    this.showDeleteModal = true;
  }

  confirmDelete(): void {
    if (this.selectedIngredient) {
      this.ingredientService.deleteIngredient(this.selectedIngredient.id).subscribe({
        next: () => {
          this.toastr.success('Ingredient deleted successfully');
          this.loadIngredients();
          this.closeDeleteModal();
        },
        error: () => {
          this.toastr.error('Error deleting ingredient');
          this.closeDeleteModal();
        }
      });
    }
  }

  closeDeleteModal() {
    this.showDeleteModal = false;
  }

  isExpiringSoon(date: Date): boolean {
    const expiryDate = new Date(date);
    const today = new Date();
    const daysUntilExpiry = Math.floor(
      (expiryDate.getTime() - today.getTime()) / (1000 * 60 * 60 * 24)
    );
    return daysUntilExpiry <= 7;
  }
}
