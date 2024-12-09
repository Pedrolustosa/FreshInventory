import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { tap, catchError } from "rxjs/operators";
import { environment } from "../../environments/environment";
import {
  UserCreateDto,
  UserLoginDto,
  UserLoginResponseDto,
  UserUpdateDto,
  UserReadDto,
} from "../models/auth.model";
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
        catchError(this.handleError("Registration failed"))
      );
  }

  login(credentials: UserLoginDto): Observable<UserLoginResponseDto> {
    return this.http
      .post<UserLoginResponseDto>(`${this.apiUrl}/LoginUser`, credentials)
      .pipe(
        tap((response) => {
          this.setCurrentUser(response);
        }),
        catchError(this.handleError("Invalid login credentials"))
      );
  }

  updateUser(id: string, userData: UserUpdateDto): Observable<UserReadDto> {
    return this.http
      .put<UserReadDto>(`${this.apiUrl}/UpdateUserProfile?userId=${id}`, userData)
      .pipe(
        tap((response) => {
          this.toastr.success("User updated successfully!");
        }),
        catchError(this.handleError("Failed to update user"))
      );
  }

  getUserById(id: string): Observable<UserReadDto> {
    return this.http
      .get<UserReadDto>(`${this.apiUrl}/GetById/${id}`)
      .pipe(
        catchError(this.handleError("Failed to fetch user by ID"))
      );
  }

  getUserByEmail(email: string): Observable<UserReadDto> {
    return this.http
      .get<UserReadDto>(`${this.apiUrl}/GetByEmail/${email}`)
      .pipe(
        catchError(this.handleError("Failed to fetch user by email"))
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

  private handleError(defaultMessage: string) {
    return (error: any) => {
      const errorMessage = error.error?.message || defaultMessage;
      this.toastr.error(errorMessage);
      return throwError(() => new Error(errorMessage));
    };
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
