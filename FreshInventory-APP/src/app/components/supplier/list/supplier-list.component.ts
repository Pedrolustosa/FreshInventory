import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SupplierService } from '../../../services/supplier.service';
import { Supplier } from '../../../models/supplier.model';
import { DeleteConfirmationModalComponent } from '../modals/delete-confirmation-modal.component';

@Component({
  selector: 'app-supplier-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    NgxSpinnerModule,
    DeleteConfirmationModalComponent
  ],
  templateUrl: './supplier-list.component.html',
  styleUrls: ['./supplier-list.component.css']
})
export class SupplierListComponent implements OnInit {
  suppliers: Supplier[] = [];
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  searchName = '';
  showDeleteModal = false;
  selectedSupplier: Supplier | null = null;
  protected readonly Math = Math;

  constructor(
    private supplierService: SupplierService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers(): void {
    this.supplierService.getSuppliers(
      this.currentPage,
      this.pageSize,
      this.searchName
    ).subscribe({
      next: (response) => {
        this.suppliers = response.items;
        this.totalItems = response.totalCount;
      },
      error: () => {
        this.toastr.error('Error loading suppliers');
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadSuppliers();
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadSuppliers();
  }

  onDeleteClick(supplier: Supplier): void {
    this.selectedSupplier = supplier;
    this.showDeleteModal = true;
  }

  onDeleteConfirm(): void {
    if (this.selectedSupplier) {
      this.supplierService.deleteSupplier(this.selectedSupplier.id).subscribe({
        next: () => {
          this.toastr.success('Supplier deleted successfully');
          this.loadSuppliers();
          this.closeDeleteModal();
        },
        error: () => {
          this.toastr.error('Error deleting supplier');
          this.closeDeleteModal();
        }
      });
    }
  }

  closeDeleteModal(): void {
    this.showDeleteModal = false;
    this.selectedSupplier = null;
  }
}