import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ModalModule } from 'ngx-bootstrap/modal';
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
    ModalModule
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
  totalPages: number = 1;
  Math = Math;
  userName: string = 'Admin'; // TODO: Get from auth service

  constructor(
    private ingredientService: IngredientService,
    private toastService: ToastService,
    private spinnerService: SpinnerService
  ) {}

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
      next: (response) => {
        this.ingredients = response.items;
        this.totalItems = response.totalItems;
        this.totalPages = Math.ceil(this.totalItems / this.pageSize);
      },
      error: (error) => {
        this.toastService.error('Failed to load ingredients. Please try again.');
        console.error('Error loading ingredients:', error);
      }
    });
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadIngredients();
  }

  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadIngredients();
    }
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 5;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);

    if (endPage - startPage + 1 < maxVisiblePages) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  getCategoryLabel(category: Category): string {
    return CategoryLabels[category];
  }

  isExpiringSoon(date: Date): boolean {
    const expiryDate = new Date(date);
    const today = new Date();
    const daysUntilExpiry = Math.ceil((expiryDate.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
    return daysUntilExpiry <= 7 && daysUntilExpiry >= 0;
  }

  openDeleteModal(ingredient: Ingredient): void {
    this.selectedIngredient = ingredient;
    const modal = document.getElementById('deleteModal');
    if (modal) {
      const bsModal = new Modal(modal);
      bsModal.show();
    }
  }

  deleteIngredient(): void {
    if (!this.selectedIngredient) return;

    this.spinnerService.show();
    this.ingredientService.deleteIngredient(this.selectedIngredient.id)
      .pipe(
        finalize(() => {
          this.spinnerService.hide();
          const modal = document.getElementById('deleteModal');
          if (modal) {
            const bsModal = Modal.getInstance(modal);
            bsModal?.hide();
          }
        })
      )
      .subscribe({
        next: () => {
          this.toastService.success('Ingredient deleted successfully');
          this.loadIngredients();
        },
        error: (error) => {
          this.toastService.error('Failed to delete ingredient. Please try again.');
          console.error('Error deleting ingredient:', error);
        }
      });
  }
}
