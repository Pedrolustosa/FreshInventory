import { Unit, UnitLabels } from "../../../models/enums/unit.enum";
import { Category, CategoryLabels } from "../../../models/enums/category.enum";
import { Ingredient } from "../../../models/ingredient.model";
import { Component, OnInit } from "@angular/core";
import { IngredientFormBase } from "../shared/ingredient-form.base";
import { IngredientService } from "src/app/services/ingredient.service";
import { SupplierService } from "src/app/services/supplier.service";
import { ActivatedRoute, Router, RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgxSpinnerModule } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: "app-ingredient-update",
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    BsDatepickerModule,
    TooltipModule,
    BsDropdownModule
  ],
  templateUrl: "./ingredient-update.component.html",
  styleUrls: ["./ingredient-update.component.css"]
})
export class IngredientUpdateComponent extends IngredientFormBase implements OnInit {
  // Get only numeric values from enums
  override units = Object.values(Unit).filter(value => typeof value === 'number') as Unit[];
  override categories = Object.values(Category).filter(value => typeof value === 'number') as Category[];
  override categoryLabels = CategoryLabels;
  override unitLabels = UnitLabels;
  isLoading = false;
  maxDate = new Date();
  minDate = new Date(new Date().setFullYear(new Date().getFullYear() - 1));
  bsConfig = {
    dateInputFormat: 'DD/MM/YYYY',
    containerClass: 'theme-default',
    showWeekNumbers: false,
    adaptivePosition: true
  };

  constructor(
    protected override ingredientService: IngredientService,
    protected override supplierService: SupplierService,
    protected override router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    super(ingredientService, supplierService, router);
  }

  ngOnInit(): void {
    this.loadIngredient();
  }

  private loadIngredient(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.isLoading = true;
      this.spinner.show();
      this.ingredientService.getIngredientById(id).subscribe({
        next: (ingredient: Ingredient) => {
          console.log('Ingredient from API:', ingredient);
          console.log('Unit type:', typeof ingredient.unit);
          console.log('Category type:', typeof ingredient.category);
          
          // Convert enum string names to their numeric values
          const formValue = {
            ...ingredient,
            unit: typeof ingredient.unit === 'string' ? this.getUnitValue(ingredient.unit) : ingredient.unit,
            category: typeof ingredient.category === 'string' ? this.getCategoryValue(ingredient.category) : ingredient.category,
            purchaseDate: new Date(ingredient.purchaseDate),
            expiryDate: ingredient.expiryDate ? new Date(ingredient.expiryDate) : null
          };
          
          console.log('Form values after conversion:', formValue);
          this.ingredientForm.patchValue(formValue);
          
          this.isLoading = false;
          this.spinner.hide();
        },
        error: (error: any) => {
          console.error('Error loading ingredient:', error);
          this.toastr.error('Failed to load ingredient details');
          this.isLoading = false;
          this.spinner.hide();
          this.router.navigate(['/ingredients']);
        }
      });
    }
  }

  onSubmit(): void {
    if (this.ingredientForm.valid) {
      this.isLoading = true;
      this.spinner.show();
      const id = Number(this.route.snapshot.paramMap.get('id'));
      const formData = this.ingredientForm.value;
      formData.id = id;

      this.ingredientService.updateIngredient(id, formData).subscribe({
        next: () => {
          this.toastr.success('Ingredient updated successfully');
          this.isLoading = false;
          this.spinner.hide();
          this.router.navigate(['/ingredients']);
        },
        error: (error: any) => {
          console.error('Error updating ingredient:', error);
          this.toastr.error('Failed to update ingredient');
          this.isLoading = false;
          this.spinner.hide();
        }
      });
    } else {
      this.markFormGroupTouched(this.ingredientForm);
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

  // Helper methods to convert enum string names to values
  private getUnitValue(unitName: string): Unit {
    const unitEntry = Object.entries(Unit).find(([key]) => key === unitName);
    return unitEntry ? unitEntry[1] as Unit : Unit.Unit; // Default to Unit if not found
  }

  private getCategoryValue(categoryName: string): Category {
    const categoryEntry = Object.entries(Category).find(([key]) => key === categoryName);
    return categoryEntry ? categoryEntry[1] as Category : Category.Other; // Default to Other if not found
  }
}
