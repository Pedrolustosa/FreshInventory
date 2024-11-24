import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { NgxSpinnerModule } from 'ngx-spinner';
import { Router } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule, 
    NgxSpinnerModule,
    BsDatepickerModule
  ],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  isEditing = false;
  maxDate = new Date();

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.profileForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      dateOfBirth: [null, Validators.required]
    });

    // Set max date to 18 years ago
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  ngOnInit(): void {
    const currentUser = this.authService.currentUserValue;
    if (currentUser) {
      this.profileForm.patchValue({
        fullName: currentUser.fullName,
        email: currentUser.email,
        dateOfBirth: new Date(currentUser.dateOfBirth)
      });
    } else {
      this.toastr.error('User session not found');
      this.router.navigate(['/auth/login']);
    }
    this.profileForm.disable();
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.profileForm.enable();
      this.profileForm.get('email')?.disable(); // Email cannot be changed
    } else {
      this.profileForm.disable();
    }
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      const currentUser = this.authService.currentUserValue;
      if (!currentUser?.id) {
        this.toastr.error('User session not found');
        this.router.navigate(['/auth/login']);
        return;
      }

      const updateData = {
        id: currentUser.id,
        fullName: this.profileForm.get('fullName')?.value,
        dateOfBirth: this.profileForm.get('dateOfBirth')?.value,
        email: currentUser.email
      };

      this.authService.updateUser(updateData).subscribe({
        next: () => {
          this.toggleEdit();
        },
        error: () => {
          // Error handling is done in the service
        }
      });
    } else {
      this.markFormGroupTouched(this.profileForm);
      this.toastr.warning('Please fill in all required fields');
    }
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