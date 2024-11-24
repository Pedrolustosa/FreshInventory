import { Component, EventEmitter, Output, OnInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { AuthService } from '../../services/auth.service';
import { AuthResponse } from '../../models/auth.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule, BsDropdownModule],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  isCollapsed = false;
  currentUser: AuthResponse | null = null;

  constructor(
    private authService: AuthService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user) => {
      this.currentUser = user;
    });

    if (!this.currentUser) {
      this.authService.fetchUserProfile();
    }
  }

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
  }

  onLogout() {
    this.spinner.show();
    this.authService.logout();
    setTimeout(() => {
      this.spinner.hide();
      this.router.navigate(['/auth/login']);
    }, 1000);
  }
}
