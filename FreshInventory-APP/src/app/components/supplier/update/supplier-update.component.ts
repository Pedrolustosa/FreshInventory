import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SupplierService } from '../../../services/supplier.service';
import { SupplierFormBase } from '../shared/supplier-form.base';

@Component({
  selector: 'app-supplier-update',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule
  ],
  templateUrl: './supplier-update.component.html',
  styleUrls: ['./supplier-update.component.css']
})
export class SupplierUpdateComponent extends SupplierFormBase implements OnInit {
  supplierId: number = 0;

  constructor(
    protected override supplierService: SupplierService,
    protected override router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) {
    super(supplierService, router);
  }

  ngOnInit(): void {
    this.supplierId = Number(this.route.snapshot.params['id']);
    this.loadSupplier();
  }

  private loadSupplier(): void {
    this.supplierService.getSupplierById(this.supplierId).subscribe({
      next: (supplier) => {
        this.supplierForm.patchValue(supplier);
      },
      error: () => {
        this.toastr.error('Error loading supplier');
        this.router.navigate(['/suppliers']);
      }
    });
  }

  override onSubmit(): void {
    if (this.supplierForm.valid) {
      const updateData = {
        ...this.supplierForm.value,
        id: this.supplierId
      };

      this.supplierService.updateSupplier(this.supplierId, updateData).subscribe({
        next: () => {
          this.toastr.success('Supplier updated successfully');
          this.router.navigate(['/suppliers']);
        },
        error: () => {
          this.toastr.error('Error updating supplier');
        }
      });
    } else {
      this.markFormGroupTouched(this.supplierForm);
      this.toastr.warning('Please fill in all required fields');
    }
  }
}