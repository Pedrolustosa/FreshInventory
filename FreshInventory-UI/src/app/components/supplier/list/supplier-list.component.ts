import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxSpinnerModule } from 'ngx-spinner';
import { SupplierService } from '../../../services/supplier.service';
import { ToastService } from '../../../services/toast.service';
import { SpinnerService } from '../../../services/spinner.service';
import { Supplier } from '../../../models/supplier.model';
import { finalize } from 'rxjs/operators';
import { Modal } from 'bootstrap';

@Component({
  selector: 'app-supplier-list',
  templateUrl: './supplier-list.component.html',
  styleUrls: ['./supplier-list.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    TooltipModule,
    ModalModule,
    NgxSpinnerModule
  ]
})
export class SupplierListComponent implements OnInit {
  suppliers: Supplier[] = [];
  selectedSupplier: Supplier | null = null;
  searchName: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  totalPages: number = 1;
  Math = Math;
  private deleteModal?: Modal;

  constructor(
    private supplierService: SupplierService,
    private toastService: ToastService,
    private spinnerService: SpinnerService
  ) {}

  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers(): void {
    this.spinnerService.show();
    this.supplierService.getSuppliers(this.currentPage, this.pageSize, this.searchName)
      .pipe(finalize(() => this.spinnerService.hide()))
      .subscribe({
        next: (response) => {
          this.suppliers = response.items;
          this.totalItems = response.totalItems;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        },
        error: () => {
          this.toastService.error('Failed to load suppliers. Please try again.');
        }
      });
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadSuppliers();
  }

  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.currentPage = page;
      this.loadSuppliers();
    }
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPages = 5;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxPages / 2));
    let endPage = Math.min(this.totalPages, startPage + maxPages - 1);

    if (endPage - startPage + 1 < maxPages) {
      startPage = Math.max(1, endPage - maxPages + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
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