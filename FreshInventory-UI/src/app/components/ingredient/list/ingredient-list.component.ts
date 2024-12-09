import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { IngredientService } from '../../../services/ingredient.service';
import { IngredientReadDto } from '../../../models/ingredient.model';
import { finalize } from 'rxjs/operators';
import { Modal } from 'bootstrap';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

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
  ingredients: IngredientReadDto[] = [];
  searchName: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  selectedIngredient: IngredientReadDto | null = null;
  Math = Math; // Adiciona Math para uso no template

  constructor(
    private ingredientService: IngredientService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.loadIngredients();
  }

  loadIngredients(): void {
    this.spinner.show();
    this.ingredientService
      .getAllIngredientsPaged(this.currentPage, this.pageSize)
      .pipe(finalize(() => this.spinner.hide()))
      .subscribe({
        next: (response) => {
          this.ingredients = response.data || [];
          this.totalItems = response.totalCount || 0;
        },
        error: () => {
          this.toastr.error('Failed to load ingredients. Please try again.');
        },
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

  openDeleteModal(ingredient: IngredientReadDto): void {
    this.selectedIngredient = ingredient;
    const modalElement = document.getElementById('deleteModal');
    if (modalElement) {
      Modal.getOrCreateInstance(modalElement).show();
    }
  }

  deleteIngredient(): void {
    if (!this.selectedIngredient) return;

    this.spinner.show();
    this.ingredientService.deleteIngredient(this.selectedIngredient.id)
      .pipe(finalize(() => this.spinner.hide()))
      .subscribe({
        next: () => {
          this.toastr.success('Ingredient deleted successfully');
          this.loadIngredients();
          this.selectedIngredient = null;
        },
        error: () => {
          this.toastr.error('Failed to delete ingredient. Please try again.');
        },
      });
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(value);
  }
}
