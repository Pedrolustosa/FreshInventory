export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  dateOfBirth: Date;
  password: string;
}

export interface UpdateUserRequest {
  id: string;
  fullName: string;
  dateOfBirth: Date;
  email: string;
}

export interface AuthResponse {
  id: string;
  fullName: string;
  email: string;
  dateOfBirth: Date;
  token: string;
}

export interface AuthError {
  message: string;
  errors?: { [key: string]: string[] };
}