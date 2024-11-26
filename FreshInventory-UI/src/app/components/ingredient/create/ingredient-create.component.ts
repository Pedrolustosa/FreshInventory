import { Component, OnInit } from "@angular/core";
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { IngredientFormBase } from "../shared/ingredient-form.base";
import { CommonModule } from "@angular/common";
import { Router, RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { IngredientService } from "src/app/services/ingredient.service";
import { SupplierService } from "src/app/services/supplier.service";
import { CreateIngredient } from "src/app/models/ingredient.model";
import { Unit, UnitLabels } from "src/app/models/enums/unit.enum";
import { Category, CategoryLabels } from "src/app/models/enums/category.enum";
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
  selector: "app-ingredient-create",
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    BsDatepickerModule
  ],
  templateUrl: "./ingredient-create.component.html",
  styleUrls: ["./ingredient-create.component.css"],
})
export class IngredientCreateComponent extends IngredientFormBase implements OnInit {
  override units = Object.values(Unit).filter((value) => typeof value === 'number') as Unit[];
  override categories = Object.values(Category).filter((value) => typeof value === 'number') as Category[];
  override categoryLabels = CategoryLabels;
  override unitLabels = UnitLabels;
  isLoading = false;
  maxDate = new Date();
  minDate = new Date(new Date().setFullYear(new Date().getFullYear() - 1));
  bsConfig = {
    dateInputFormat: 'YYYY-MM-DD',
    containerClass: 'theme-default',
    adaptivePosition: true,
    showWeekNumbers: false
  };

  constructor(
    protected override ingredientService: IngredientService,
    protected override supplierService: SupplierService,
    protected override router: Router,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {
    super(ingredientService, supplierService, router);
  }

  ngOnInit(): void {}

  override onSubmit(): void {
    if (this.ingredientForm.valid) {
      this.isLoading = true;
      this.spinner.show();

      const formData = this.ingredientForm.value;
      const ingredient: CreateIngredient = {
        ...formData,
        unit: Number(formData.unit),
        category: Number(formData.category),
        purchaseDate: formData.purchaseDate,
        expiryDate: formData.expiryDate
      };

      this.ingredientService.createIngredient(ingredient).subscribe({
        next: () => {
          this.isLoading = false;
          this.spinner.hide();
          this.toastr.success('Ingredient created successfully!');
          this.router.navigate(['/ingredients']);
        },
        error: (error: any) => {
          console.error('Error creating ingredient:', error);
          this.isLoading = false;
          this.spinner.hide();
          this.toastr.error('Failed to create ingredient. Please try again.');
        }
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
      if (control.errors['required']) return `${controlName.charAt(0).toUpperCase() + controlName.slice(1)} is required`;
      if (control.errors['min']) return `Value must be at least ${control.errors['min'].min}`;
      if (control.errors['max']) return `Value must be at most ${control.errors['max'].max}`;
    }
    return '';
  }
}
