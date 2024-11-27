import { Unit, UnitLabels } from "../../../models/enums/unit.enum";
import { Category, CategoryLabels } from "../../../models/enums/category.enum";
import { Ingredient, UpdateIngredient } from "../../../models/ingredient.model";
import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { IngredientService } from "src/app/services/ingredient.service";
import { SupplierService } from "src/app/services/supplier.service";
import { ActivatedRoute, Router, RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Supplier } from "src/app/models/supplier.model";

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
export class IngredientUpdateComponent implements OnInit {
  ingredientForm!: FormGroup;
  units = Object.values(Unit).filter(value => typeof value === 'number') as Unit[];
  categories = Object.values(Category).filter(value => typeof value === 'number') as Category[];
  categoryLabels = CategoryLabels;
  unitLabels = UnitLabels;
  suppliers: Supplier[] = [];
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
    private ingredientService: IngredientService,
    private supplierService: SupplierService,
    private router: Router,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {
    this.createForm();
    this.loadSuppliers();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.loadIngredient(id);
      }
    });
  }

  private createForm(): void {
    this.ingredientForm = this.formBuilder.group({
      id: [''],
      name: ['', [Validators.required]],
      quantity: ['', [Validators.required, Validators.min(0)]],
      unit: [null, [Validators.required]],
      unitCost: ['', [Validators.required, Validators.min(0)]],
      category: [null, [Validators.required]],
      supplierId: ['', [Validators.required]],
      purchaseDate: ['', [Validators.required]],
      expiryDate: ['', [Validators.required]],
      isPerishable: [false],
      reorderLevel: ['', [Validators.required, Validators.min(0)]]
    });
  }

  private loadSuppliers(): void {
    this.supplierService.getSuppliers(1, 100, "", "name", "asc").subscribe({
      next: (response: any) => {
        this.suppliers = response.items.filter(
          (supplier: Supplier) => supplier.status
        );
      },
      error: (error: any) => {
        console.error("Error loading suppliers:", error);
        this.toastr.error("Failed to load suppliers");
      }
    });
  }

  private loadIngredient(id: number): void {
    this.spinner.show();
    this.ingredientService.getIngredientById(id).subscribe({
      next: (ingredient: Ingredient) => {

        // Convert string/number values to enum values
        const unitValue = typeof ingredient.unit === 'string' 
          ? Unit[ingredient.unit as keyof typeof Unit]
          : ingredient.unit;
        
        const categoryValue = typeof ingredient.category === 'string'
          ? Category[ingredient.category as keyof typeof Category]
          : ingredient.category;

        // Set form values with the correct enum values
        const formValues = {
          ...ingredient,
          unit: unitValue,
          category: categoryValue,
          purchaseDate: new Date(ingredient.purchaseDate),
          expiryDate: new Date(ingredient.expiryDate)
        };
        
        this.ingredientForm.patchValue(formValues);
        this.spinner.hide();
      },
      error: (error: any) => {
        console.error('Error loading ingredient:', error);
        this.toastr.error('Failed to load ingredient');
        this.spinner.hide();
      }
    });
  }

  onSubmit(): void {
    if (this.ingredientForm.valid) {
      this.spinner.show();
      const formData = this.ingredientForm.value;
      const ingredient: UpdateIngredient = {
        ...formData,
        unit: Number(formData.unit),
        category: Number(formData.category)
      };

      this.ingredientService.updateIngredient(ingredient.id, ingredient).subscribe({
        next: () => {
          this.spinner.hide();
          this.toastr.success('Ingredient updated successfully!');
          this.router.navigate(['/ingredients']);
        },
        error: (error: any) => {
          console.error('Error updating ingredient:', error);
          this.spinner.hide();
          this.toastr.error('Failed to update ingredient');
        }
      });
    } else {
      this.markFormGroupTouched(this.ingredientForm);
      this.toastr.warning('Please fill in all required fields');
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

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}
