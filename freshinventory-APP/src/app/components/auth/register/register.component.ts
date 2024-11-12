import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  showPassword = false;
  showConfirmPassword = false;
  isLoading = false;
  passwordStrength = 0;
  passwordCriteria = {
    length: false,
    lowercase: false,
    uppercase: false,
    number: false,
    special: false
  };

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.registerForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)
      ]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validator: this.passwordMatchValidator
    });

    // Subscribe to password changes to update strength
    this.registerForm.get('password')?.valueChanges.subscribe(password => {
      this.updatePasswordStrength(password);
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      this.isLoading = true;
      this.authService.register(this.registerForm.value).subscribe({
        next: () => {
          this.toastr.success('Registration successful! Welcome to Fresh Inventory.');
          this.router.navigate(['/dashboard']);
          this.isLoading = false;
        },
        error: (error) => {
          this.toastr.error(error.error?.message || 'Registration failed. Please try again.');
          this.isLoading = false;
        }
      });
    } else {
      this.markFormGroupTouched(this.registerForm);
      this.toastr.warning('Please fill in all required fields correctly.');
    }
  }

  onCancel(): void {
    this.router.navigate(['/auth/login']);
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  getFieldError(fieldName: string): string {
    const control = this.registerForm.get(fieldName);
    if (control?.errors && control.touched) {
      if (control.errors['required']) return `${fieldName} is required`;
      if (control.errors['email']) return 'Please enter a valid email address';
      if (control.errors['minlength']) return `${fieldName} must be at least ${control.errors['minlength'].requiredLength} characters`;
      if (control.errors['pattern']) return 'Please enter a valid format';
      if (control.errors['mismatch']) return 'Passwords do not match';
    }
    return '';
  }

  private updatePasswordStrength(password: string): void {
    this.passwordCriteria = {
      length: password.length >= 8,
      lowercase: /[a-z]/.test(password),
      uppercase: /[A-Z]/.test(password),
      number: /\d/.test(password),
      special: /[@$!%*?&]/.test(password)
    };

    const criteriaCount = Object.values(this.passwordCriteria).filter(Boolean).length;
    this.passwordStrength = (criteriaCount / 5) * 100;
  }

  getPasswordStrengthClass(): string {
    if (this.passwordStrength <= 20) return 'weak';
    if (this.passwordStrength <= 60) return 'medium';
    return 'strong';
  }

  getPasswordStrengthText(): string {
    if (this.passwordStrength <= 20) return 'Weak';
    if (this.passwordStrength <= 60) return 'Medium';
    return 'Strong';
  }

  private passwordMatchValidator(g: FormGroup) {
    return g.get('password')?.value === g.get('confirmPassword')?.value
      ? null
      : { mismatch: true };
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}