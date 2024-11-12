import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { IngredientService } from '../../../services/ingredient.service';
import { Ingredient } from '../../../models/ingredient.model';
import { DeleteConfirmationModalComponent } from '../modals/delete-confirmation-modal.component';

@Component({
  selector: 'app-ingredient-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    NgxSpinnerModule,
    DatePipe,
    DeleteConfirmationModalComponent
  ],
  templateUrl: './ingredient-list.component.html',
  styleUrls: ['./ingredient-list.component.css']
})
export class IngredientListComponent implements OnInit {
  ingredients: Ingredient[] = [];
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  searchName = '';
  selectedCategory = '';
  showDeleteModal = false;
  selectedIngredient: Ingredient | null = null;
  protected readonly Math = Math;

  constructor(
    private ingredientService: IngredientService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadIngredients();
  }

  loadIngredients(): void {
    this.ingredientService.getIngredients(
      this.currentPage,
      this.pageSize,
      this.searchName,
      this.selectedCategory
    ).subscribe({
      next: (response) => {
        this.ingredients = response.items;
        this.totalItems = response.totalCount;
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadIngredients();
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadIngredients();
  }

  onDeleteClick(ingredient: Ingredient): void {
    this.selectedIngredient = ingredient;
    this.showDeleteModal = true;
  }

  onDeleteConfirm(): void {
    if (this.selectedIngredient) {
      this.ingredientService.deleteIngredient(this.selectedIngredient.id).subscribe({
        next: () => {
          this.toastr.success('Ingredient deleted successfully');
          this.loadIngredients();
          this.closeDeleteModal();
        },
        error: () => {
          this.toastr.error('Error deleting ingredient');
          this.closeDeleteModal();
        }
      });
    }
  }

  closeDeleteModal(): void {
    this.showDeleteModal = false;
    this.selectedIngredient = null;
  }

  isExpiringSoon(date: Date): boolean {
    const expiryDate = new Date(date);
    const today = new Date();
    const daysUntilExpiry = Math.floor((expiryDate.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
    return daysUntilExpiry <= 7;
  }
}