import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { finalize } from 'rxjs/operators';
import { UpdateUserDto } from '../../models/auth.model';

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
  profileImage: string | null = null;
  private originalFormValues: any;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router,
    private spinner: NgxSpinnerService
  ) {
    this.profileForm = this.fb.group({
      id: [''],
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      dateOfBirth: [null, Validators.required],
      password: ['']
    });

    // Set max date to 18 years ago
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  ngOnInit(): void {
    this.loadUserProfile();
  }

  loadUserProfile(): void {
    this.spinner.show();
    const currentUser = this.authService.currentUserValue;
    
    if (currentUser?.email) {
      // Busca os dados mais recentes do usuário usando o email
      this.authService.getUserByEmail(currentUser.email)
        .pipe(finalize(() => this.spinner.hide()))
        .subscribe({
          next: (user) => {
            if (user) {
              this.profileForm.patchValue({
                id: user.id,
                fullName: user.fullName,
                email: user.email,
                dateOfBirth: new Date(user.dateOfBirth)
              });
              this.profileImage = user.profileImage || null;
              this.originalFormValues = this.profileForm.value;
            }
          },
          error: () => {
            this.toastr.error('Erro ao carregar dados do perfil');
            this.router.navigate(['/login']);
          }
        });
    } else {
      this.spinner.hide();
      this.toastr.error('Usuário não encontrado');
      this.router.navigate(['/login']);
    }
  }

  toggleEdit(): void {
    this.isEditing = true;
    this.originalFormValues = this.profileForm.value;
  }

  cancelEdit(): void {
    this.profileForm.patchValue(this.originalFormValues);
    this.isEditing = false;
  }

  onUploadImage(): void {
    this.toastr.info('Upload image');
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      this.spinner.show();

      // Primeiro busca os dados mais recentes do usuário
      const email = this.profileForm.get('email')?.value;
      
      this.authService.getUserByEmail(email)
        .pipe(finalize(() => this.spinner.hide()))
        .subscribe({
          next: (user) => {
            const updatedProfile: UpdateUserDto = {
              id: user.id,
              fullName: this.profileForm.get('fullName')?.value,
              email: this.profileForm.get('email')?.value,
              dateOfBirth: this.profileForm.get('dateOfBirth')?.value?.toISOString(),
              password: this.profileForm.get('password')?.value || undefined
            };

            this.spinner.show();
            this.authService.updateUser(updatedProfile)
              .pipe(finalize(() => this.spinner.hide()))
              .subscribe({
                next: (success) => {
                  if (success) {
                    this.toastr.success('Perfil atualizado com sucesso');
                    this.isEditing = false;
                    this.originalFormValues = this.profileForm.value;
                    if (this.profileForm.get('password')?.value) {
                      this.profileForm.patchValue({ password: '' });
                    }
                    // Recarrega os dados do perfil após a atualização
                    this.loadUserProfile();
                  } else {
                    this.toastr.error('Não foi possível atualizar o perfil');
                  }
                },
                error: (error) => {
                  this.toastr.error(error.message || 'Erro ao atualizar perfil');
                }
              });
          },
          error: () => {
            this.toastr.error('Erro ao buscar dados atualizados do usuário');
          }
        });
    } else {
      Object.keys(this.profileForm.controls).forEach(key => {
        const control = this.profileForm.get(key);
        if (control?.invalid) {
          control.markAsTouched();
        }
      });
    }
  }
}