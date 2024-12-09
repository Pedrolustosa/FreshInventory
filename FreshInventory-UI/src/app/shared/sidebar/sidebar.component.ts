import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { AuthService } from '../../services/auth.service';
import { IngredientService } from '../../services/ingredient.service';
import { RecipeService } from '../../services/recipe.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subject, takeUntil } from 'rxjs';
import { UserLoginResponseDto } from '../../models/auth.model';

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
  private destroy$ = new Subject<void>();
  
  currentUser: UserLoginResponseDto | null = null; // Propriedade adicionada para armazenar o usuário logado.

  constructor(
    private authService: AuthService,
    private ingredientService: IngredientService,
    private recipeService: RecipeService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadUserProfile();
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
          this.currentUser = user; // Armazena o usuário logado.
          if (!user) {
            this.currentUser = this.authService.currentUserValue; // Usa o valor atual do usuário.
          }
        },
        error: (error) => {
          console.error('Error loading user profile:', error);
        }
      });
  }

  toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
    this.isCollapsedChange.emit(this.isCollapsed);
  }

  async logout(): Promise<void> {
    try {
      this.spinner.show();
      this.authService.logout(); // Chama o método de logout.
      await this.router.navigate(['/auth/login']);
    } catch (error) {
      console.error('Error during logout:', error);
    } finally {
      this.spinner.hide();
    }
  }
}
