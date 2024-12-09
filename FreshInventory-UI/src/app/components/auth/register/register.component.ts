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
import { UserCreateDto } from "../../../models/auth.model";

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
    isAnimated: true,
  };
  passwordCriteria = {
    length: false,
    lowercase: false,
    uppercase: false,
    number: false,
    special: false,
    specialChar: false,
  };
  genderOptions = [
    { value: 0, label: "Male" },
    { value: 1, label: "Female" },
  ];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    this.registerForm = this.fb.group(
      {
        fullName: ["", [Validators.required, Validators.minLength(3)]],
        userName: ["", [Validators.required, Validators.minLength(3)]],
        email: ["", [Validators.required, Validators.email]],
        password: ["", [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)]],
        confirmPassword: ["", [Validators.required]],
        dateOfBirth: [null, [Validators.required]],
        street: ["", [Validators.required, Validators.minLength(3)]],
        city: ["", [Validators.required, Validators.minLength(2)]],
        state: ["", [Validators.required, Validators.minLength(2)]],
        zipCode: ["", [Validators.required, Validators.minLength(5)]],
        country: ["", [Validators.required, Validators.minLength(2)]],
        bio: ["", [Validators.maxLength(500)]],
        alternatePhoneNumber: ["", [Validators.pattern("^[0-9]{10,15}$")]],
        gender: [null, [Validators.required]],
        nationality: ["", [Validators.required, Validators.minLength(2)]],
        languagePreference: ["", [Validators.required, Validators.minLength(2)]],
        timeZone: [""],
      },
      {
        validator: this.passwordMatchValidator("password", "confirmPassword"),
      }
    );

    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);

    this.registerForm.get("password")?.valueChanges.subscribe((password) => {
      this.updatePasswordStrength(password || "");
    });
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
    const criteriaCount = Object.values(this.passwordCriteria).filter(Boolean).length;
    this.passwordStrength = (criteriaCount / 6) * 100;
  }

  getPasswordStrengthText(): string {
    if (this.passwordStrength <= 40) return "Weak";
    if (this.passwordStrength <= 80) return "Medium";
    return "Strong";
  }

  getFieldError(fieldName: string): string {
    const control = this.registerForm.get(fieldName);
    if (control?.errors && (control.touched || this.submitted)) {
      if (control.errors["required"]) return `${this.formatFieldName(fieldName)} is required`;
      if (control.errors["email"]) return "Please enter a valid email address";
      if (control.errors["minlength"]) return `${this.formatFieldName(fieldName)} must be at least ${control.errors["minlength"].requiredLength} characters`;
      if (control.errors["pattern"]) {
        if (fieldName === "password") return "Password must contain at least 8 characters, including uppercase, lowercase, number and special character";
        return `Invalid ${this.formatFieldName(fieldName)} format`;
      }
      if (control.errors["mismatch"]) return "Passwords do not match";
    }
    return "";
  }

  private formatFieldName(fieldName: string): string {
    const fieldNames: Record<string, string> = {
      fullName: "Full Name",
      userName: "Username",
      email: "Email Address",
      dateOfBirth: "Date of Birth",
      password: "Password",
      confirmPassword: "Confirm Password",
      street: "Street",
      city: "City",
      state: "State",
      zipCode: "Zip Code",
      country: "Country",
      bio: "Bio",
      alternatePhoneNumber: "Alternate Phone Number",
      gender: "Gender",
      nationality: "Nationality",
      languagePreference: "Language Preference",
    };
    return fieldNames[fieldName] || fieldName;
  }  

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
      const registerData: UserCreateDto = { ...formValue };

      this.authService.register(registerData).subscribe({
        next: () => {
          this.spinner.hide();
          this.toastr.success("Registration successful! Welcome to Fresh Inventory.");
          this.router.navigate(["/auth/login"]);
        },
        error: (error) => {
          this.spinner.hide();
          this.toastr.error(error.error?.message || "Registration failed. Please try again.");
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

  private passwordMatchValidator(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];
      if (matchingControl.errors && !matchingControl.errors["mismatch"]) return;
      matchingControl.setErrors(control.value !== matchingControl.value ? { mismatch: true } : null);
    };
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      if (control instanceof FormGroup) this.markFormGroupTouched(control);
    });
  }
}
