import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { IngredientService } from '../../../services/ingredient.service';
import { ToastService } from '../../../services/toast.service';
import { SpinnerService } from '../../../services/spinner.service';
import { Ingredient } from '../../../models/ingredient.model';
import { Category, CategoryLabels } from '../../../models/enums/category.enum';
import { finalize } from 'rxjs/operators';
import { Modal } from 'bootstrap';

@Component({
  selector: 'app-ingredient-list',
  templateUrl: './ingredient-list.component.html',
  styleUrls: ['./ingredient-list.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    TooltipModule,
    ModalModule,
    PaginationModule
  ]
})
export class IngredientListComponent implements OnInit {
  ingredients: Ingredient[] = [];
  categories = Object.values(Category).filter(v => typeof v === 'number') as Category[];
  categoryLabels = CategoryLabels;
  selectedIngredient: Ingredient | null = null;
  searchName: string = '';
  selectedCategory: Category | null = null;
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  maxSize: number = 5;
  Math = Math;
  private deleteModal?: Modal;

  // Constants for expiry validation
  readonly EXPIRY_WARNING_DAYS = 30;
  readonly EXPIRY_DANGER_DAYS = 7;

  constructor(
    private ingredientService: IngredientService,
    private toastService: ToastService,
    private spinnerService: SpinnerService
  ) { }

  ngOnInit(): void {
    this.loadIngredients();
  }

  loadIngredients(): void {
    this.spinnerService.show();
    this.ingredientService.getIngredients(
      this.currentPage,
      this.pageSize,
      this.searchName,
      this.selectedCategory?.toString()
    )
    .pipe(
      finalize(() => this.spinnerService.hide())
    )
    .subscribe({
      next: (response: any) => {
        console.log('API Response:', response); // Debug log
        if (response) {
          this.ingredients = response.items || [];
          this.totalItems = response.totalCount || 0;
          this.currentPage = response.currentPage || 1;
        }
      },
      error: (error) => {
        console.error('Error loading ingredients:', error);
        this.toastService.error('Failed to load ingredients. Please try again.');
      }
    });
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadIngredients();
  }

  pageChanged(event: any): void {
    if (event && event.page !== this.currentPage) {
      this.currentPage = event.page;
      this.loadIngredients();
    }
  }

  openDeleteModal(ingredient: Ingredient): void {
    this.selectedIngredient = ingredient;
    const modalElement = document.getElementById('deleteModal');
    if (modalElement) {
      // Garantir que não há instância anterior do modal
      Modal.getOrCreateInstance(modalElement).show();
    }
  }

  deleteIngredient(): void {
    if (!this.selectedIngredient) return;

    this.spinnerService.show();
    this.ingredientService.deleteIngredient(this.selectedIngredient.id)
      .pipe(
        finalize(() => {
          this.spinnerService.hide();
          // Fecha o modal após a operação
          const modalElement = document.getElementById('deleteModal');
          if (modalElement) {
            const modal = Modal.getInstance(modalElement);
            if (modal) {
              modal.hide();
              // Remove o backdrop manualmente se necessário
              const backdrop = document.querySelector('.modal-backdrop');
              if (backdrop) {
                backdrop.remove();
              }
            }
          }
        })
      )
      .subscribe({
        next: () => {
          this.toastService.success('Ingredient deleted successfully');
          this.loadIngredients();
          this.selectedIngredient = null; // Limpa a seleção
        },
        error: (error) => {
          console.error('Error deleting ingredient:', error);
          this.toastService.error(error?.error || 'Failed to delete ingredient. Please try again.');
        }
      });
  }

  getCategoryLabel(category: Category): string {
    return CategoryLabels[category];
  }

  getExpiryStatus(expiryDate: string | Date | null): { status: string; label: string } {
    if (!expiryDate) {
      return { status: 'secondary', label: 'No expiry date' };
    }

    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    const expiry = new Date(expiryDate);
    expiry.setHours(0, 0, 0, 0);
    
    const daysUntilExpiry = Math.ceil((expiry.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));

    if (daysUntilExpiry < 0) {
      return { status: 'danger', label: 'Expired' };
    } else if (daysUntilExpiry <= this.EXPIRY_DANGER_DAYS) {
      return { status: 'danger', label: `Expires in ${daysUntilExpiry} day${daysUntilExpiry === 1 ? '' : 's'}` };
    } else if (daysUntilExpiry <= this.EXPIRY_WARNING_DAYS) {
      return { status: 'warning', label: `Expires in ${daysUntilExpiry} days` };
    } else {
      return { status: 'success', label: `Valid for ${daysUntilExpiry} days` };
    }
  }

  formatDate(date: string | Date | null): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString();
  }

  getQuantityStatus(quantity: number, reorderLevel: number): string {
    if (quantity <= 0) {
      return 'danger';
    } else if (quantity <= reorderLevel) {
      return 'warning';
    }
    return 'success';
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(value);
  }
}
