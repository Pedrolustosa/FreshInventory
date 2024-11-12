import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  isEditing = false;

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService
  ) {
    this.profileForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.pattern(/^\+?[\d\s-]+$/)],
      position: [''],
      company: [''],
      bio: ['']
    });
  }

  ngOnInit(): void {
    // Mock data - replace with actual user data from service
    this.profileForm.patchValue({
      name: 'John Doe',
      email: 'john.doe@example.com',
      phone: '+1 (555) 123-4567',
      position: 'Kitchen Manager',
      company: 'Fresh Foods Inc.',
      bio: 'Experienced kitchen manager with a passion for efficient inventory management.'
    });
    this.profileForm.disable();
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.profileForm.enable();
    } else {
      this.profileForm.disable();
    }
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      // Save profile changes
      this.toastr.success('Profile updated successfully');
      this.toggleEdit();
    } else {
      this.toastr.error('Please check the form for errors');
    }
  }
}