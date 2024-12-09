import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SupplierService } from '../../../services/supplier.service';

@Component({
  selector: 'app-supplier-create',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
  ],
  templateUrl: './supplier-create.component.html',
  styleUrls: ['./supplier-create.component.css'],
})
export class SupplierCreateComponent implements OnInit {
  supplierForm!: FormGroup;
  isLoading = false;

  constructor(
    private formBuilder: FormBuilder,
    private supplierService: SupplierService,
    private router: Router,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.createForm();
  }

  createForm(): void {
    this.supplierForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      contact: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9\\-+()\\s]+$')]],
      address: ['', [Validators.required, Validators.minLength(10)]],
      category: ['', [Validators.required]],
      status: [true], // Default status to true
    });
  }

  getControlError(controlName: string): string {
    const control = this.supplierForm.get(controlName);
    if (control?.errors && control.touched) {
      if (control.errors['required']) return `${controlName.charAt(0).toUpperCase() + controlName.slice(1)} is required.`;
      if (control.errors['minlength']) return `${controlName} must be at least ${control.errors['minlength'].requiredLength} characters.`;
      if (control.errors['pattern']) return `Invalid ${controlName} format.`;
      if (control.errors['email']) return 'Invalid email format.';
    }
    return '';
  }

  onSubmit(): void {
    if (this.supplierForm.valid) {
      this.spinner.show();
      const supplierData = this.supplierForm.value;

      this.supplierService.createSupplier(supplierData).subscribe({
        next: () => {
          this.spinner.hide();
          this.toastr.success('Supplier created successfully!');
          this.router.navigate(['/suppliers']);
        },
        error: (error: any) => {
          console.error('Error creating supplier:', error);
          this.spinner.hide();
          this.toastr.error('Failed to create supplier. Please try again.');
        },
      });
    } else {
      this.markFormGroupTouched(this.supplierForm);
      this.toastr.warning('Please fill in all required fields correctly.');
    }
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
