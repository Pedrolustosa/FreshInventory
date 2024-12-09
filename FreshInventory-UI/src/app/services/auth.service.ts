import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { tap, catchError } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { UserCreateDto, UserLoginDto, UserLoginResponseDto, UserUpdateDto } from "../models/auth.model";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/api/auth`;
  private currentUserSubject = new BehaviorSubject<UserLoginResponseDto | null>(null);
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

  register(userData: UserCreateDto): Observable<UserLoginResponseDto> {
    return this.http
      .post<UserLoginResponseDto>(`${this.apiUrl}/RegisterUser`, userData)
      .pipe(
        tap((response) => {
          this.setCurrentUser(response);
          this.toastr.success("Registration successful!");
          this.router.navigate(["/dashboard"]);
        }),
        catchError((error) => {
          const errorMessage = error.error?.message || "Registration failed";
          this.toastr.error(errorMessage);
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  login(credentials: UserLoginDto): Observable<UserLoginResponseDto> {
    return this.http
      .post<UserLoginResponseDto>(`${this.apiUrl}/LoginUser`, credentials)
      .pipe(
        tap((response) => {
          this.setCurrentUser(response);
        }),
        catchError((error) => {
          const errorMessage = error.error?.message || "Invalid login credentials";
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  updateUser(userId: string, userData: UserUpdateDto): Observable<UserLoginResponseDto> {
    return this.http
      .put<UserLoginResponseDto>(`${this.apiUrl}/UpdateUserProfile?userId=${userId}`, userData)
      .pipe(
        tap((response) => {
          this.setCurrentUser(response);
          this.toastr.success("User updated successfully!");
        }),
        catchError((error) => {
          const errorMessage = error.error?.message || "Failed to update user";
          this.toastr.error(errorMessage);
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  getUserById(userId: string): Observable<UserLoginResponseDto> {
    return this.http
      .get<UserLoginResponseDto>(`${this.apiUrl}/GetById/${userId}`)
      .pipe(
        tap((response) => {
          this.currentUserSubject.next(response);
        }),
        catchError((error) => {
          const errorMessage = error.error?.message || "Failed to fetch user by ID";
          this.toastr.error(errorMessage);
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  getUserByEmail(email: string): Observable<UserLoginResponseDto> {
    return this.http
      .get<UserLoginResponseDto>(`${this.apiUrl}/GetByEmail/${email}`)
      .pipe(
        tap((response) => {
          const currentUser = this.currentUserSubject.value;
          if (currentUser) {
            this.currentUserSubject.next({ ...currentUser, ...response });
            localStorage.setItem("currentUser", JSON.stringify(this.currentUserSubject.value));
          }
        }),
        catchError((error) => {
          const errorMessage = error.error?.message || "Failed to fetch user by email";
          this.toastr.error(errorMessage);
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  logout(): void {
    localStorage.removeItem("currentUser");
    this.currentUserSubject.next(null);
    this.toastr.info("You have been logged out");
    this.router.navigate(["/auth/login"]);
  }

  private setCurrentUser(user: UserLoginResponseDto): void {
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

  get currentUserValue(): UserLoginResponseDto | null {
    return this.currentUserSubject.value;
  }

  get token(): string | null {
    return this.currentUserValue?.token ?? null;
  }
}
