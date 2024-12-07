import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SupplierService } from '../../../services/supplier.service';
import { Supplier } from '../../../models/supplier.model';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { finalize } from 'rxjs/operators';
import { Modal } from 'bootstrap';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-supplier-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    NgxSpinnerModule,
    NgxMaskPipe,
    PaginationModule
  ],
  templateUrl: './supplier-list.component.html',
  styleUrls: ['./supplier-list.component.css']
})
export class SupplierListComponent implements OnInit {
  suppliers: Supplier[] = [];
  selectedSupplier: Supplier | null = null;
  searchName: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  maxSize: number = 5;
  Math = Math;
  private deleteModal?: Modal;

  constructor(
    private supplierService: SupplierService,
    private toastService: ToastrService,
    private spinnerService: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers(): void {
    this.spinnerService.show();
    this.supplierService.getSuppliers(this.currentPage, this.pageSize, this.searchName)
      .pipe(finalize(() => this.spinnerService.hide()))
      .subscribe({
        next: (response: any) => {
          console.log('API Response:', response); // Debug log
          if (response) {
            this.suppliers = response.items || [];
            this.totalItems = response.totalCount || 0;
            this.currentPage = response.currentPage || 1;
          }
        },
        error: (error) => {
          console.error('Error loading suppliers:', error);
          this.toastService.error('Failed to load suppliers. Please try again.');
        }
      });
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadSuppliers();
  }

  pageChanged(event: any): void {
    if (event && event.page !== this.currentPage) {
      this.currentPage = event.page;
      this.loadSuppliers();
    }
  }

  openDeleteModal(supplier: Supplier): void {
    this.selectedSupplier = supplier;
    const modalElement = document.getElementById('deleteModal');
    if (modalElement) {
      this.deleteModal = new Modal(modalElement);
      this.deleteModal.show();
    }
  }

  deleteSupplier(): void {
    if (this.selectedSupplier) {
      this.spinnerService.show();
      this.supplierService.deleteSupplier(this.selectedSupplier.id)
        .pipe(finalize(() => this.spinnerService.hide()))
        .subscribe({
          next: () => {
            this.toastService.success('Supplier deleted successfully!');
            this.deleteModal?.hide();
            this.loadSuppliers();
          },
          error: () => {
            this.toastService.error('Failed to delete supplier. Please try again.');
          }
        });
    }
  }
}