import {
  Component,
  EventEmitter,
  Output,
  OnInit,
  ChangeDetectorRef,
} from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule, Router } from "@angular/router";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { AuthService } from "../../services/auth.service";
import { AuthResponse } from "../../models/auth.model";

@Component({
  selector: "app-sidebar",
  standalone: true,
  imports: [CommonModule, RouterModule, BsDropdownModule],
  templateUrl: "./sidebar.component.html",
  styleUrls: ["./sidebar.component.css"],
})
export class SidebarComponent implements OnInit {
  isCollapsed = false;
  currentUser: AuthResponse | null = null;
  showUserMenu = false;
  isDropup = true;

  @Output() collapseChange = new EventEmitter<boolean>();

  constructor(
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((authResponse) => {
      this.currentUser = authResponse;
      this.cdr.detectChanges();
    });
    if (!this.currentUser) {
      this.authService.fetchUserProfile();
    }
  }

  toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
    this.collapseChange.emit(this.isCollapsed);
  }

  toggleUserMenu(): void {
    this.showUserMenu = !this.showUserMenu;
  }

  onProfileClick(): void {
    this.router.navigate(["/profile"]);
    this.showUserMenu = false;
  }

  onLogout(): void {
    this.authService.logout();
    this.showUserMenu = false;
  }
}
