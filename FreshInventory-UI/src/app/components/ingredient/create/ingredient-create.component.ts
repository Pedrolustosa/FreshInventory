import { Component, OnInit } from "@angular/core";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { IngredientFormBase } from "../shared/ingredient-form.base";
import { CommonModule } from "@angular/common";
import { Router, RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { IngredientService } from "src/app/services/ingredient.service";
import { SupplierService } from "src/app/services/supplier.service";
import { IngredientCreateDto } from "src/app/models/ingredient.model";
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";

@Component({
  selector: "app-ingredient-create",
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    BsDatepickerModule,
  ],
  templateUrl: "./ingredient-create.component.html",
  styleUrls: ["./ingredient-create.component.css"],
})
export class IngredientCreateComponent extends IngredientFormBase implements OnInit {
  isLoading = false;

  constructor(
    protected override ingredientService: IngredientService,
    protected override supplierService: SupplierService,
    protected override router: Router,
    public override toastr: ToastrService, // Corrigido para usar ToastrService
    public override spinner: NgxSpinnerService // Corrigido para usar NgxSpinnerService
  ) {
    super(ingredientService, supplierService, router, toastr, spinner);
  }

  ngOnInit(): void {}

  override onSubmit(): void {
    if (this.ingredientForm.valid) {
      this.isLoading = true;
      this.spinner.show();

      const formData = this.ingredientForm.value;
      const ingredient: IngredientCreateDto = {
        name: formData.name,
        quantity: formData.quantity,
        unitCost: formData.unitCost,
        supplierId: formData.supplierId,
      };

      this.ingredientService.createIngredient(ingredient).subscribe({
        next: () => {
          this.isLoading = false;
          this.spinner.hide();
          this.toastr.success("Ingredient created successfully!");
          this.router.navigate(["/ingredients"]);
        },
        error: (error: any) => {
          console.error("Error creating ingredient:", error);
          this.isLoading = false;
          this.spinner.hide();
          this.toastr.error("Failed to create ingredient. Please try again.");
        },
      });
    } else {
      this.markFormGroupTouched(this.ingredientForm);
      this.toastr.warning(
        "Please fill in all required fields.",
        "Form Incomplete"
      );
    }
  }

  getControlError(controlName: string): string {
    const control = this.ingredientForm.get(controlName);
    if (control?.errors && control.touched) {
      if (control.errors["required"])
        return `${controlName.charAt(0).toUpperCase() + controlName.slice(1)} is required`;
      if (control.errors["min"])
        return `Value must be at least ${control.errors["min"].min}`;
    }
    return "";
  }
}
