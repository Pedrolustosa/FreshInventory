import { Unit } from "../../../models/enums/unit.enum";
import { Category } from "../../../models/enums/category.enum";
import { Component, OnInit } from "@angular/core";
import { IngredientFormBase } from "../shared/ingredient-form.base";
import { IngredientService } from "src/app/services/ingredient.service";
import { SupplierService } from "src/app/services/supplier.service";
import { ActivatedRoute, Router, RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: "app-ingredient-update",
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
  ],
  templateUrl: "./ingredient-update.component.html",
  styleUrls: ["./ingredient-update.component.css"],
})
export class IngredientUpdateComponent
  extends IngredientFormBase
  implements OnInit
{
  ingredientId: number = 0;

  override units: Unit[] = Object.values(Unit) as Unit[];
  override categories: Category[] = Object.values(Category) as Category[];

  constructor(
    protected override ingredientService: IngredientService,
    protected override supplierService: SupplierService,
    protected override router: Router,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {
    super(ingredientService, supplierService, router);
  }

  ngOnInit(): void {
    this.ingredientId = this.route.snapshot.params["id"];
    this.loadIngredient();
  }

  private loadIngredient(): void {
    this.spinner.show();
    this.ingredientService.getIngredientById(this.ingredientId).subscribe({
      next: (ingredient) => {
        this.ingredientForm.patchValue({
          ...ingredient,
          unit: ingredient.unit as Unit,
          category: ingredient.category as Category,
          purchaseDate: this.formatDate(ingredient.purchaseDate),
          expiryDate: this.formatDate(ingredient.expiryDate),
        });
        this.spinner.hide();
      },
      error: (error) => {
        this.spinner.hide();
        this.toastr.error("Failed to load ingredient data.", "Error");
        this.router.navigate(["/ingredients"]);
      },
    });
  }

  override onSubmit(): void {
    if (this.ingredientForm.valid) {
      const ingredientUpdate = {
        ...this.ingredientForm.value,
        id: this.ingredientId,
      };
      this.spinner.show();
      this.ingredientService
        .updateIngredient(this.ingredientId, ingredientUpdate)
        .subscribe({
          next: () => {
            this.spinner.hide();
            this.toastr.success("Ingredient updated successfully!", "Success");
            this.router.navigate(["/ingredients"]);
          },
          error: (error) => {
            this.spinner.hide();
            console.error("Error updating ingredient:", error);
            this.toastr.error(
              "Failed to update ingredient. Please try again.",
              "Error"
            );
          },
        });
    } else {
      this.markFormGroupTouched(this.ingredientForm);
      this.toastr.warning(
        "Please fix the errors in the form before submitting.",
        "Warning"
      );
    }
  }
}
