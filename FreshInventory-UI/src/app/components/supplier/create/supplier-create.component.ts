import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SupplierService } from '../../../services/supplier.service';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-supplier-create',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    BsDatepickerModule
  ],
  templateUrl: './supplier-create.component.html',
  styleUrls: ['./supplier-create.component.css']
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
      code: ['', [Validators.required, Validators.pattern('^[A-Za-z0-9-]{3,10}$')]],
      contactName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9-+()\\s]{10,}$')]],
      address: ['', [Validators.required, Validators.minLength(10)]],
      website: ['', [Validators.pattern('^https?://.*')]],
      isActive: [true]
    });
  }

  getControlError(controlName: string): string {
    const control = this.supplierForm.get(controlName);
    if (control?.invalid && (control.dirty || control.touched)) {
      if (control.errors?.['required']) {
        return `${this.getFieldLabel(controlName)} is required`;
      }
      if (control.errors?.['minlength']) {
        return `${this.getFieldLabel(controlName)} must be at least ${control.errors['minlength']['requiredLength']} characters`;
      }
      if (control.errors?.['email']) {
        return 'Please enter a valid email address';
      }
      if (control.errors?.['pattern']) {
        switch (controlName) {
          case 'code':
            return 'Code must be 3-10 characters (letters, numbers, or hyphens)';
          case 'phone':
            return 'Please enter a valid phone number';
          case 'website':
            return 'Please enter a valid website URL (starting with http:// or https://)';
          default:
            return 'Invalid format';
        }
      }
    }
    return '';
  }

  private getFieldLabel(controlName: string): string {
    const labels: { [key: string]: string } = {
      name: 'Company Name',
      code: 'Supplier Code',
      contactName: 'Contact Name',
      email: 'Email',
      phone: 'Phone',
      address: 'Address',
      website: 'Website'
    };
    return labels[controlName] || controlName;
  }

  async onSubmit(): Promise<void> {
    if (this.supplierForm.valid && !this.isLoading) {
      this.isLoading = true;
      this.spinner.show();

      try {
        await this.supplierService.createSupplier(this.supplierForm.value).toPromise();
        this.toastr.success('Supplier created successfully!', 'Success');
        this.router.navigate(['/suppliers']);
      } catch (error) {
        console.error('Error creating supplier:', error);
        this.toastr.error('Failed to create supplier. Please try again.', 'Error');
      } finally {
        this.isLoading = false;
        this.spinner.hide();
      }
    } else {
      Object.keys(this.supplierForm.controls).forEach(key => {
        const control = this.supplierForm.get(key);
        if (control?.invalid) {
          control.markAsTouched();
        }
      });
    }
  }
}