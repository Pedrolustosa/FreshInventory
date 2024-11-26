import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { AuthService } from '../../services/auth.service';
import { IngredientService } from '../../services/ingredient.service';
import { RecipeService } from '../../services/recipe.service';
import { AuthResponse } from '../../models/auth.model';
import { Ingredient } from '../../models/ingredient.model';
import { Recipe } from '../../models/recipe.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    BsDropdownModule,
    TooltipModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit, OnDestroy {
  @Input() isCollapsed = false;
  @Output() isCollapsedChange = new EventEmitter<boolean>();
  currentUser: AuthResponse | null = null;
  ingredients: Ingredient[] = [];
  recipes: Recipe[] = [];
  private destroy$ = new Subject<void>();

  constructor(
    private authService: AuthService,
    private ingredientService: IngredientService,
    private recipeService: RecipeService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadUserProfile();
    this.loadData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadUserProfile(): void {
    this.authService.currentUser$
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (user) => {
          this.currentUser = user;
          if (!user) {
            this.authService.fetchUserProfile();
          }
        },
        error: (error) => {
          console.error('Error loading user profile:', error);
        }
      });
  }

  private loadData(): void {
    // Carregar ingredientes
    this.ingredientService.getIngredients()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (ingredients) => this.ingredients = ingredients,
        error: (error) => console.error('Error loading ingredients:', error)
      });

    // Carregar receitas
    this.recipeService.getRecipes()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (recipes) => this.recipes = recipes,
        error: (error) => console.error('Error loading recipes:', error)
      });
  }

  toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
    this.isCollapsedChange.emit(this.isCollapsed);
  }

  async logout(): Promise<void> {
    try {
      this.spinner.show();
      await this.authService.logout();
      await this.router.navigate(['/auth/login']);
    } catch (error) {
      console.error('Error during logout:', error);
    } finally {
      this.spinner.hide();
    }
  }
}
