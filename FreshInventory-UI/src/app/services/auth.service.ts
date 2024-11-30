import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { tap, catchError, map } from "rxjs/operators";
import { environment } from "../../environments/environment";
import {
  LoginRequest,
  RegisterRequest,
  UpdateUserDto,
  AuthResponse,
  AuthError,
} from "../models/auth.model";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/api/auth`;
  private currentUserSubject = new BehaviorSubject<AuthResponse | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router,
    private toastr: ToastrService
  ) {
    const storedUser = localStorage.getItem("currentUser");
    if (storedUser) {
      try {
        const parsedUser = JSON.parse(storedUser);
        this.currentUserSubject.next(parsedUser);
      } catch (error) {
        console.error("Failed to parse stored user:", error);
        localStorage.removeItem("currentUser");
      }
    }
  }

  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap((response) => {
        this.setCurrentUser(response);
      }),
      catchError((error) => {
        const errorMessage = error.error?.message || 'Invalid login credentials';
        return throwError(() => new Error(errorMessage));
      })
    );
  }
  

  register(userData: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/register`, userData)
      .pipe(
        tap((response) => {
          this.setCurrentUser(response);
          this.toastr.success("Registration successful!");
          this.router.navigate(["/dashboard"]);
        }),
        catchError((error) => {
          const errorResponse: AuthError = error.error;
          if (errorResponse.errors) {
            Object.values(errorResponse.errors).forEach((messages) => {
              messages.forEach((message) => this.toastr.error(message));
            });
          } else {
            this.toastr.error(errorResponse.message || "Registration failed");
          }
          return throwError(() => error);
        })
      );
  }

  updateUser(userData: UpdateUserDto): Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/profile`, userData).pipe(
      tap((success) => {
        if (success) {
          // Atualiza o estado do usuário local com os novos dados
          const currentUser = this.currentUserSubject.value;
          if (currentUser) {
            const updatedUser: AuthResponse = {
              ...currentUser,
              fullName: userData.fullName,
              email: userData.email || currentUser.email,
              dateOfBirth: userData.dateOfBirth ? new Date(userData.dateOfBirth) : currentUser.dateOfBirth
            };
            
            this.currentUserSubject.next(updatedUser);
            localStorage.setItem('currentUser', JSON.stringify(updatedUser));
          }
        }
      }),
      catchError((error) => {
        const errorMessage = error.error || 'Erro ao atualizar usuário';
        this.toastr.error(errorMessage);
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  fetchUserProfile(): void {
    if (!this.token) {
      console.warn("No token available to fetch user profile");
      return;
    }
  
    this.http
      .get<AuthResponse>(`${this.apiUrl}/profile`)
      .pipe(
        tap((user) => {
          this.setCurrentUser(user);
        }),
        catchError((error) => {
          console.error("Failed to fetch user profile:", error);
          this.logout();
          return throwError(() => error);
        })
      )
      .subscribe();
  }  

  logout(): void {
    localStorage.removeItem("currentUser");
    this.currentUserSubject.next(null);
    this.toastr.info("You have been logged out");
    this.router.navigate(["/auth/login"]);
  }

  private setCurrentUser(user: AuthResponse): void {
    if (!user) {
      console.warn("Attempted to set null user");
      return;
    }
    localStorage.setItem("currentUser", JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  get isAuthenticated(): boolean {
    return !!this.currentUserSubject.value;
  }

  get currentUserValue(): AuthResponse | null {
    return this.currentUserSubject.value;
  }

  get token(): string | null {
    return this.currentUserValue?.token ?? null;
  }

  getUserByEmail(email: string): Observable<AuthResponse> {
    return this.http.get<AuthResponse>(`${this.apiUrl}/email/${email}`).pipe(
      tap((response) => {
        // Atualiza o estado do usuário atual com os dados mais recentes
        const currentUser = this.currentUserSubject.value;
        if (currentUser) {
          this.currentUserSubject.next({
            ...currentUser,
            ...response
          });
          localStorage.setItem('currentUser', JSON.stringify(this.currentUserSubject.value));
        }
      }),
      catchError((error) => {
        const errorMessage = error.error?.message || 'Erro ao buscar dados do usuário';
        this.toastr.error(errorMessage);
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
