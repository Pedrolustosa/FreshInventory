import { Component } from "@angular/core";
import { AuthService } from "../../../services/auth.service";
import { CommonModule } from "@angular/common";
import { Router, RouterModule } from "@angular/router";
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: "app-login",
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent {
  loginForm: FormGroup;
  showPassword = false;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    this.loginForm = this.fb.group({
      email: ["", [
        Validators.required,
        Validators.email,
        Validators.pattern(
          "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"
        ),
      ]],
      password: ["", [
        Validators.required,
        Validators.minLength(6),
      ]],
      rememberMe: [false],
    });
  }

  async onSubmit(): Promise<void> {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.spinner.show();

      const MIN_LOADING_TIME = 2000;
      const startTime = Date.now();

      this.authService.login(this.loginForm.value).subscribe({
        next: async () => {
          const elapsedTime = Date.now() - startTime;
          if (elapsedTime < MIN_LOADING_TIME) {
            await this.delay(MIN_LOADING_TIME - elapsedTime);
          }

          this.spinner.hide();
          this.isLoading = false;
          this.toastr.success("Login successful! Welcome back.");
          this.router.navigate(["/dashboard"]);
        },
        error: async (err) => {
          const elapsedTime = Date.now() - startTime;
          if (elapsedTime < MIN_LOADING_TIME) {
            await this.delay(MIN_LOADING_TIME - elapsedTime);
          }

          this.spinner.hide();
          this.isLoading = false;
          this.toastr.error(
            err?.error?.message || "Invalid email or password. Please try again."
          );
        },
      });
    } else {
      this.markFormGroupTouched(this.loginForm);
      this.toastr.warning(
        "Please complete all required fields correctly to proceed."
      );
    }
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  private delay(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }

  getFieldError(fieldName: string): string {
    const control = this.loginForm.get(fieldName);
    if (control?.errors && control.touched) {
      if (control.errors["required"]) {
        return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} is required.`;
      }
      if (control.errors["email"] || control.errors["pattern"]) {
        return "Please enter a valid email address.";
      }
      if (control.errors["minlength"]) {
        return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} must be at least ${control.errors["minlength"].requiredLength} characters long.`;
      }
    }
    return "";
  }
}
