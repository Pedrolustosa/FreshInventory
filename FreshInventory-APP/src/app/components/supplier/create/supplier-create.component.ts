import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SupplierService } from '../../../services/supplier.service';
import { SupplierFormBase } from '../shared/supplier-form.base';

@Component({
  selector: 'app-supplier-create',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule
  ],
  templateUrl: './supplier-create.component.html',
  styleUrls: ['./supplier-create.component.css']
})
export class SupplierCreateComponent extends SupplierFormBase {
  constructor(
    protected override supplierService: SupplierService,
    protected override router: Router,
    private toastr: ToastrService
  ) {
    super(supplierService, router);
  }

  override onSubmit(): void {
    if (this.supplierForm.valid) {
      this.supplierService.createSupplier(this.supplierForm.value).subscribe({
        next: () => {
          this.toastr.success('Supplier created successfully');
          this.router.navigate(['/suppliers']);
        },
        error: () => {
          this.toastr.error('Error creating supplier');
        }
      });
    } else {
      this.markFormGroupTouched(this.supplierForm);
      this.toastr.warning('Please fill in all required fields');
    }
  }
}