import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from '../../services/auth.service';
import { UserLoginResponseDto } from '../../models/auth.model';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    TooltipModule,
    RouterModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit, OnDestroy {
  @Input() isCollapsed = false;
  @Output() isCollapsedChange = new EventEmitter<boolean>();
  private destroy$ = new Subject<void>();
  currentUser: UserLoginResponseDto | null = null;

  menuItems = [
    { label: 'Home', link: '/home', icon: 'fas fa-home' },
    { label: 'Dashboard', link: '/dashboard', icon: 'fas fa-chart-line' },
    { label: 'Ingredients', link: '/ingredients', icon: 'fas fa-box' },
    { label: 'Recipes', link: '/recipes', icon: 'fas fa-utensils' },
    { label: 'Suppliers', link: '/suppliers', icon: 'fas fa-truck' },
  ];

  constructor(private authService: AuthService) {}

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
          this.currentUser = user;
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
      this.authService.logout();
    } catch (error) {
      console.error('Error during logout:', error);
    }
  }
}
