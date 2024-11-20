import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SupplierService } from '../../../services/supplier.service';

export abstract class SupplierFormBase {
  supplierForm: FormGroup;
  categories = [
    'Produce',
    'Meat',
    'Dairy',
    'Bakery',
    'Beverages',
    'Other'
  ];

  constructor(
    protected supplierService: SupplierService,
    protected router: Router
  ) {
    this.supplierForm = new FormBuilder().group({
      name: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required]],
      address: ['', [Validators.required]],
      contactPerson: ['', [Validators.required]],
      category: ['', [Validators.required]],
      status: [true]
    });
  }

  abstract onSubmit(): void;

  protected markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}