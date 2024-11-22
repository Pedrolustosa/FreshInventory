import { Component } from "@angular/core";
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { IngredientFormBase } from "../shared/ingredient-form.base";
import { CommonModule } from "@angular/common";
import { Router, RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { IngredientService } from "../../../services/ingredient.service";
import { SupplierService } from "../../../services/supplier.service";

@Component({
  selector: "app-ingredient-create",
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
  ],
  templateUrl: "./ingredient-create.component.html",
  styleUrls: ["./ingredient-create.component.css"],
})
export class IngredientCreateComponent extends IngredientFormBase {
  constructor(
    protected override ingredientService: IngredientService,
    protected override supplierService: SupplierService,
    protected override router: Router,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {
    super(ingredientService, supplierService, router);
  }

  override onSubmit(): void {
    if (this.ingredientForm.valid) {
      this.spinner.show();
      this.ingredientService
        .createIngredient(this.ingredientForm.value)
        .subscribe({
          next: () => {
            this.spinner.hide();
            this.toastr.success("Ingredient created successfully!", "Success");
            this.router.navigate(["/ingredients"]);
          },
          error: (error) => {
            this.spinner.hide();
            console.error(error);
            this.toastr.error(
              "Failed to create ingredient. Please try again.",
              "Error"
            );
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
}
