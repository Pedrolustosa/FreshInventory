import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { UserUpdateDto, UserReadDto } from '../../models/auth.model';
import { CommonModule } from '@angular/common';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-profile',
  standalone: true, // Marca o componente como standalone
  imports: [
    CommonModule,
    ReactiveFormsModule,
    BsDatepickerModule
  ],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  isEditing = false;
  maxDate: Date = new Date();
  private originalFormValues: any;
  userId: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.getUserIdFromRoute();
    this.loadUserProfile();
  }

  private initForm(): void {
    this.profileForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      dateOfBirth: [null, Validators.required],
      street: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      zipCode: ['', Validators.required],
      country: ['', Validators.required],
      bio: [''],
      alternatePhoneNumber: [''],
      gender: [null, Validators.required],
      nationality: [''],
      languagePreference: [''],
      timeZone: ['']
    });

    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  private getUserIdFromRoute(): void {
    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.userId = params['id'];
      } else {
        this.toastr.error('User ID not found in route');
        this.router.navigate(['/login']);
      }
    });
  }

  loadUserProfile(): void {
    if (!this.userId) {
      return;
    }

    this.spinner.show();
    this.authService
      .getUserById(this.userId)
      .pipe(finalize(() => this.spinner.hide()))
      .subscribe({
        next: (userResponse) => {
          const user: UserReadDto = userResponse.user;
          this.profileForm.patchValue(user);
          this.originalFormValues = this.profileForm.value;
        },
        error: () => {
          this.toastr.error('Error loading profile data');
          this.router.navigate(['/login']);
        }
      });
  }

  toggleEdit(): void {
    this.isEditing = true;
    this.originalFormValues = this.profileForm.value;
  }

  cancelEdit(): void {
    this.profileForm.patchValue(this.originalFormValues);
    this.isEditing = false;
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      const updatedProfile: UserUpdateDto = {
        userId: this.userId,
        ...this.profileForm.value
      };

      this.spinner.show();
      this.authService
        .updateUser(this.userId, updatedProfile)
        .pipe(finalize(() => this.spinner.hide()))
        .subscribe({
          next: () => {
            this.toastr.success('Profile updated successfully!');
            this.isEditing = false;
            this.originalFormValues = this.profileForm.value;
          },
          error: (error) => {
            this.toastr.error(error.message || 'Error updating profile');
          }
        });
    } else {
      this.markFormGroupTouched(this.profileForm);
      this.toastr.warning('Please fill in all required fields correctly.');
    }
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  getControlError(controlName: string): string {
    const control = this.profileForm.get(controlName);
    if (control?.errors && control.touched) {
      if (control.errors['required']) {
        return `${controlName.charAt(0).toUpperCase() + controlName.slice(1)} is required.`;
      }
      if (control.errors['email']) {
        return 'Invalid email format.';
      }
    }
    return '';
  }
}
