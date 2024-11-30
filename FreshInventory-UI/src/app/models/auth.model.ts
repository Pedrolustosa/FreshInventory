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

export interface UserProfile {
  id: string;
  fullName: string;
  email: string;
  dateOfBirth: Date;
  phone?: string;
  zipCode?: string;
  city?: string;
  state?: string;
  address?: string;
  profileImage?: string;
}

export interface UpdateUserDto {
  id: string;
  fullName: string;
  email?: string;
  password?: string;
  dateOfBirth?: Date | string;
}

export interface AuthResponse extends UserProfile {
  token: string;
}

export interface AuthError {
  message: string;
  errors?: { [key: string]: string[] };
}