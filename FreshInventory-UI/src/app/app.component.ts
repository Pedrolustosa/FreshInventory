import { Router, NavigationEnd } from "@angular/router";
import { filter } from "rxjs/operators";
import { SidebarComponent } from "./shared/sidebar/sidebar.component";
import { NgxSpinnerModule } from "ngx-spinner";
import { AuthService } from "./services/auth.service";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterOutlet } from "@angular/router";

@Component({
  selector: "app-root",
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    SidebarComponent,
    NgxSpinnerModule,
    BsDropdownModule,
  ],
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})
export class AppComponent {
  isHomePage = false;
  isAuthPage = false;
  isSidebarCollapsed = false;

  constructor(private router: Router, public authService: AuthService) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.isHomePage = event.url === "/home" || event.url === "/";
        this.isAuthPage = event.url.includes("/auth/");
      });
  }

  onSidebarCollapseChange(isCollapsed: boolean): void {
    this.isSidebarCollapsed = isCollapsed;
  }
  

  shouldShowSidebar(): boolean {
    return (
      this.authService.isAuthenticated && !this.isHomePage && !this.isAuthPage
    );
  }
}
