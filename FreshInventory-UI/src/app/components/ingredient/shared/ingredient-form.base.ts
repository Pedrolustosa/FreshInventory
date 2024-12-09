import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { IngredientService } from "../../../services/ingredient.service";
import { SupplierService } from "../../../services/supplier.service";
import { ToastrService } from "ngx-toastr";
import { NgxSpinnerService } from "ngx-spinner";

interface Supplier {
  id: number;
  name: string;
  status: boolean;
}

export abstract class IngredientFormBase {
  ingredientForm: FormGroup;
  suppliers: Supplier[] = [];

  constructor(
    protected ingredientService: IngredientService,
    protected supplierService: SupplierService,
    protected router: Router,
    protected  toastr: ToastrService,
    protected spinner: NgxSpinnerService
  ) {
    this.ingredientForm = new FormBuilder().group({
      name: ["", [Validators.required, Validators.minLength(3)]],
      quantity: ["", [Validators.required, Validators.min(0)]],
      unitCost: ["", [Validators.required, Validators.min(0)]],
      supplierId: ["", [Validators.required]],
    });

    this.loadSuppliers();
  }

  private loadSuppliers(): void {
    this.spinner.show();
    this.supplierService.getAllSuppliersPaged(1, 100).subscribe({
      next: (response: any) => {
        this.suppliers = response.data.filter((supplier: Supplier) => supplier.status);
        this.spinner.hide();
      },
      error: () => {
        this.spinner.hide();
        this.toastr.error("Failed to load suppliers. Please try again.", "Error");
      },
    });
  }

  abstract onSubmit(): void;

  protected markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}
