import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SupplierService } from '../../../services/supplier.service';
import { SupplierCreateDto } from '../../../models/supplier.model';

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
      name: ['', [Validators.required, Validators.minLength(3)]],
      contact: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9\\-+()\\s]+$')]],
      address: ['', [Validators.required, Validators.minLength(10)]],
      category: ['', [Validators.required]],
      status: [true] // Campo opcional com valor padrão
    });
  }

  abstract onSubmit(): void;

  protected getFormattedSupplier(): SupplierCreateDto | null {
    if (this.supplierForm.valid) {
      const formValue = this.supplierForm.value;
      const supplier: SupplierCreateDto = {
        name: formValue.name.trim(),
        contact: formValue.contact.trim(),
        email: formValue.email.trim(),
        phone: formValue.phone.trim(),
        address: formValue.address.trim(),
        category: formValue.category,
        status: formValue.status
      };

      return supplier;
    } else {
      this.markFormGroupTouched(this.supplierForm);
      return null;
    }
  }

  protected markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  getControlError(controlName: string): string {
    const control = this.supplierForm.get(controlName);
    if (control?.errors && control.touched) {
      if (control.errors['required']) {
        return `${controlName.charAt(0).toUpperCase() + controlName.slice(1)} is required.`;
      }
      if (control.errors['minlength']) {
        return `${controlName} must be at least ${control.errors['minlength'].requiredLength} characters.`;
      }
      if (control.errors['email']) {
        return `Invalid email format.`;
      }
      if (control.errors['pattern']) {
        return `Invalid ${controlName} format.`;
      }
    }
    return '';
  }
}
