import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { IngredientService } from "src/app/services/ingredient.service";
import { SupplierService } from "src/app/services/supplier.service";
import { ActivatedRoute, Router, RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { SupplierReadDto } from "src/app/models/supplier.model";
import { IngredientReadDto, IngredientUpdateDto } from "../../../models/ingredient.model";

@Component({
  selector: "app-ingredient-update",
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: "./ingredient-update.component.html",
  styleUrls: ["./ingredient-update.component.css"],
})
export class IngredientUpdateComponent implements OnInit {
  ingredientForm!: FormGroup;
  suppliers: SupplierReadDto[] = [];
  isLoading = false;
  ingredientId: number | null = null;

  constructor(
    private ingredientService: IngredientService,
    private supplierService: SupplierService,
    private router: Router,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.createForm();
    this.loadSuppliers();
    this.route.params.subscribe((params) => {
      this.ingredientId = +params["id"];
      if (this.ingredientId) {
        this.loadIngredient(this.ingredientId);
      }
    });
  }

  private createForm(): void {
    this.ingredientForm = this.formBuilder.group({
      name: ["", [Validators.required, Validators.minLength(3)]],
      quantity: [0, [Validators.required, Validators.min(1)]],
      unitCost: [0, [Validators.required, Validators.min(0)]],
      supplierId: [null, [Validators.required]],
    });
  }

  private loadSuppliers(): void {
    this.spinner.show();
    this.supplierService.getAllSuppliersPaged(1, 100).subscribe({
      next: (response) => {
        this.suppliers = response.data || [];
        this.spinner.hide();
      },
      error: (error: any) => {
        console.error("Error loading suppliers:", error);
        this.toastr.error("Failed to load suppliers.");
        this.spinner.hide();
      },
    });
  }

  private loadIngredient(id: number): void {
    this.spinner.show();
    this.ingredientService.getIngredientById(id).subscribe({
      next: (ingredient: IngredientReadDto) => {
        this.ingredientForm.patchValue(ingredient);
        this.spinner.hide();
      },
      error: (error: any) => {
        console.error("Error loading ingredient:", error);
        this.toastr.error("Failed to load ingredient.");
        this.spinner.hide();
      },
    });
  }

  onSubmit(): void {
    if (this.ingredientForm.valid && this.ingredientId) {
      this.spinner.show();
      const formData = this.ingredientForm.value;
      const ingredient: IngredientUpdateDto = {
        name: formData.name,
        quantity: formData.quantity,
        unitCost: formData.unitCost,
        supplierId: formData.supplierId,
      };

      this.ingredientService.updateIngredient(this.ingredientId, ingredient).subscribe({
        next: () => {
          this.toastr.success("Ingredient updated successfully!");
          this.spinner.hide();
          this.router.navigate(["/ingredients"]);
        },
        error: (error: any) => {
          console.error("Error updating ingredient:", error);
          this.toastr.error("Failed to update ingredient.");
          this.spinner.hide();
        },
      });
    } else {
      this.markFormGroupTouched(this.ingredientForm);
      this.toastr.warning("Please fill in all required fields.");
    }
  }

  getControlError(controlName: string): string {
    const control = this.ingredientForm.get(controlName);
    if (control?.errors && control.touched) {
      if (control.errors["required"]) return `${controlName.charAt(0).toUpperCase() + controlName.slice(1)} is required.`;
      if (control.errors["min"]) return `Value must be at least ${control.errors["min"].min}.`;
    }
    return "";
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}
