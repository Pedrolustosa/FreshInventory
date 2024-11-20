import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  showPassword = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [false]
    });
  }

  async onSubmit(): Promise<void> {
    if (this.loginForm.valid) {
      this.spinner.show();

      const MIN_LOADING_TIME = 2000; // 2 segundos
      const startTime = Date.now();

      this.authService.login(this.loginForm.value).subscribe({
        next: async () => {
          const elapsedTime = Date.now() - startTime;

          // Aguarda o tempo mínimo se a requisição for muito rápida
          if (elapsedTime < MIN_LOADING_TIME) {
            await this.delay(MIN_LOADING_TIME - elapsedTime);
          }

          this.spinner.hide();
          this.toastr.success('Login successful! Welcome back.');
          this.router.navigate(['/dashboard']);
        },
        error: async (err) => {
          const elapsedTime = Date.now() - startTime;

          // Aguarda o tempo mínimo se a requisição for muito rápida
          if (elapsedTime < MIN_LOADING_TIME) {
            await this.delay(MIN_LOADING_TIME - elapsedTime);
          }

          this.spinner.hide();
          this.toastr.error(err?.error?.message || 'Invalid credentials');
        }
      });
    } else {
      this.markFormGroupTouched(this.loginForm);
      this.toastr.warning('Please fill out the form correctly.');
    }
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  private delay(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}
