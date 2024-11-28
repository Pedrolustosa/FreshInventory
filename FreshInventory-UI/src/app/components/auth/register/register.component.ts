import { AuthService } from "../../../services/auth.service";
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";
import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { Router, RouterModule } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: "app-register",
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    BsDatepickerModule,
  ],
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.css"],
})
export class RegisterComponent {
  registerForm: FormGroup;
  showPassword = false;
  isLoading = false;
  passwordStrength = 0;
  maxDate = new Date();
  submitted = false;
  bsConfig = {
    containerClass: 'theme-default',
    dateInputFormat: 'DD/MM/YYYY',
    showWeekNumbers: false,
    adaptivePosition: true,
    isAnimated: true
  };
  passwordCriteria = {
    length: false,
    lowercase: false,
    uppercase: false,
    number: false,
    special: false,
    specialChar: false
  };

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    this.registerForm = this.fb.group({
      fullName: ["", [Validators.required, Validators.minLength(3)]],
      email: [
        "",
        [
          Validators.required,
          Validators.email,
          Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$"),
        ],
      ],
      dateOfBirth: [null, [Validators.required]],
      password: [
        "",
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/
          ),
        ],
      ],
      confirmPassword: ["", [Validators.required]]
    }, {
      validator: this.passwordMatchValidator('password', 'confirmPassword')
    });

    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);

    this.registerForm.get("password")?.valueChanges.subscribe((password) => {
      this.updatePasswordStrength(password || "");
    });
  }

  // Getter for easy access to form fields
  get f() {
    return this.registerForm.controls;
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    this.submitted = true;
    
    if (this.registerForm.valid) {
      this.spinner.show();
      const formValue = this.registerForm.value;
      const registerData = {
        fullName: formValue.fullName,
        email: formValue.email,
        dateOfBirth: formValue.dateOfBirth,
        password: formValue.password,
      };

      this.authService.register(registerData).subscribe({
        next: () => {
          this.spinner.hide();
          this.toastr.success(
            "Registration successful! Welcome to Fresh Inventory."
          );
          this.router.navigate(["/auth/login"]);
        },
        error: (error) => {
          this.spinner.hide();
          this.toastr.error(
            error.error?.message || "Registration failed. Please try again."
          );
        },
      });
    } else {
      this.markFormGroupTouched(this.registerForm);
      this.toastr.warning("Please fill in all required fields correctly.");
    }
  }

  onCancel(): void {
    this.router.navigate(["/auth/login"]);
  }

  getFieldError(fieldName: string): string {
    const control = this.registerForm.get(fieldName);
    if (control?.errors && (control.touched || this.submitted)) {
      if (control.errors["required"]) {
        return `${this.formatFieldName(fieldName)} is required`;
      }
      if (control.errors["email"]) {
        return `Please enter a valid email address`;
      }
      if (control.errors["minlength"]) {
        const requiredLength = control.errors["minlength"].requiredLength;
        return `${this.formatFieldName(fieldName)} must be at least ${requiredLength} characters`;
      }
      if (control.errors["pattern"]) {
        if (fieldName === 'password') {
          return 'Password must contain at least 8 characters, including uppercase, lowercase, number and special character';
        }
        return `Invalid ${this.formatFieldName(fieldName)} format`;
      }
    }
    return "";
  }

  private formatFieldName(fieldName: string): string {
    switch (fieldName) {
      case "fullName":
        return "Full Name";
      case "email":
        return "Email Address";
      case "dateOfBirth":
        return "Date of Birth";
      case "password":
        return "Password";
      case "confirmPassword":
        return "Confirm Password";
      default:
        return fieldName;
    }
  }

  updatePasswordStrength(password: string): void {
    this.passwordCriteria = {
      length: password.length >= 8,
      lowercase: /[a-z]/.test(password),
      uppercase: /[A-Z]/.test(password),
      number: /\d/.test(password),
      special: /[@$!%*?&]/.test(password),
      specialChar: /[@$!%*?&]/.test(password),
    };

    const criteriaCount = Object.values(this.passwordCriteria).filter(
      Boolean
    ).length;
    this.passwordStrength = (criteriaCount / 6) * 100;
  }

  getPasswordStrengthText(): string {
    if (this.passwordStrength <= 40) {
      return "Weak";
    } else if (this.passwordStrength > 40 && this.passwordStrength <= 80) {
      return "Medium";
    } else {
      return "Strong";
    }
  }

  private passwordMatchValidator(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (matchingControl.errors && !matchingControl.errors["mismatch"]) {
        return;
      }

      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mismatch: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}
