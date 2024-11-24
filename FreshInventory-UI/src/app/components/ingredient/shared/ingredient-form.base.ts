import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { IngredientService } from "../../../services/ingredient.service";
import { SupplierService } from "../../../services/supplier.service";
import { Unit, UnitLabels } from "../../../models/enums/unit.enum";
import { Category, CategoryLabels } from "../../../models/enums/category.enum";
import { Supplier } from "../../../models/supplier.model";

export abstract class IngredientFormBase {
  ingredientForm: FormGroup;
  units = Object.keys(Unit)
    .filter((key) => !isNaN(Number(key)))
    .map((key) => Number(key) as Unit);
  categories = Object.keys(Category)
    .filter((key) => !isNaN(Number(key)))
    .map((key) => Number(key) as Category);
  unitLabels = UnitLabels;
  categoryLabels = CategoryLabels;
  suppliers: Supplier[] = [];

  constructor(
    protected ingredientService: IngredientService,
    protected supplierService: SupplierService,
    protected router: Router
  ) {
    this.ingredientForm = new FormBuilder().group({
      name: ["", [Validators.required]],
      quantity: ["", [Validators.required, Validators.min(0)]],
      unit: [null, [Validators.required]],
      unitCost: ["", [Validators.required, Validators.min(0)]],
      category: [null, [Validators.required]],
      supplierId: ["", [Validators.required]],
      purchaseDate: ["", [Validators.required]],
      expiryDate: ["", [Validators.required]],
      isPerishable: [false],
      reorderLevel: ["", [Validators.required, Validators.min(0)]],
    });

    this.loadSuppliers();
  }

  private loadSuppliers(): void {
    this.supplierService.getSuppliers(1, 100, "", "name", "asc").subscribe({
      next: (response) => {
        this.suppliers = response.items.filter(
          (supplier: Supplier) => supplier.status
        );
      },
      error: (error) => {
        console.error("Error loading suppliers:", error);
      },
    });
  }

  getUnitLabel(unit: Unit): string {
    return UnitLabels[unit];
  }

  getCategoryLabel(category: Category): string {
    return CategoryLabels[category];
  }

  abstract onSubmit(): void;

  protected formatDate(date: string | Date): string {
    const formattedDate = new Date(date).toISOString().split("T")[0];
    return formattedDate;
  }

  protected markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}
