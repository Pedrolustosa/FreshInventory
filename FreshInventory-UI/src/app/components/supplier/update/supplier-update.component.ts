import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SupplierService } from '../../../services/supplier.service';
import { NgxMaskDirective } from 'ngx-mask';
import { phoneNumberValidator } from '../../../shared/validators/phone.validator';
import { SupplierReadDto, SupplierUpdateDto } from '../../../models/supplier.model';

@Component({
  selector: 'app-supplier-update',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    NgxMaskDirective
  ],
  templateUrl: './supplier-update.component.html',
  styleUrls: ['./supplier-update.component.css']
})
export class SupplierUpdateComponent implements OnInit {
  supplierForm!: FormGroup;
  isLoading = false;
  supplierId!: number;

  constructor(
    private formBuilder: FormBuilder,
    private supplierService: SupplierService,
    private router: Router,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.createForm();
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.supplierId = +params['id'];
        this.loadSupplier(this.supplierId);
      }
    });
  }

  createForm(): void {
    this.supplierForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      contact: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, phoneNumberValidator]],
      address: ['', [Validators.required, Validators.minLength(10)]],
      category: ['', [Validators.required]],
      status: [true, [Validators.required]]
    });
  }

  loadSupplier(id: number): void {
    this.spinner.show();
    this.supplierService.getSupplierById(id).subscribe({
      next: (supplier: SupplierReadDto) => {
        this.supplierForm.patchValue({
          name: supplier.name,
          contact: supplier.contact,
          email: supplier.email,
          phone: supplier.phone,
          address: supplier.address,
          category: supplier.category,
          status: supplier.status
        });
        this.spinner.hide();
      },
      error: (error: any) => {
        console.error('Error loading supplier:', error);
        this.spinner.hide();
        this.toastr.error('Failed to load supplier');
        this.router.navigate(['/suppliers']);
      }
    });
  }

  getControlError(controlName: string): string {
    const control = this.supplierForm.get(controlName);
    if (control?.errors && control.touched) {
      if (control.errors['required']) return `${controlName.charAt(0).toUpperCase() + controlName.slice(1)} is required`;
      if (control.errors['minlength']) return `${controlName} must be at least ${control.errors['minlength'].requiredLength} characters`;
      if (control.errors['email']) return 'Invalid email format';
      if (control.errors['invalidPhone']) return 'Invalid phone number format';
    }
    return '';
  }

  onSubmit(): void {
    if (this.supplierForm.valid) {
      this.spinner.show();
      const supplier: SupplierUpdateDto = this.supplierForm.value;
      this.supplierService.updateSupplier(this.supplierId, supplier).subscribe({
        next: () => {
          this.spinner.hide();
          this.toastr.success('Supplier updated successfully!');
          this.router.navigate(['/suppliers']);
        },
        error: (error: any) => {
          console.error('Error updating supplier:', error);
          this.spinner.hide();
          this.toastr.error('Failed to update supplier');
        }
      });
    } else {
      this.markFormGroupTouched(this.supplierForm);
      this.toastr.warning('Please fill in all required fields correctly');
    }
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
