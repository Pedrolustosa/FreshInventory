// auth.model.ts

export interface UserCreateDto {
  fullName: string;
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
  dateOfBirth?: Date;
  street: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  bio: string;
  alternatePhoneNumber: string;
  gender: number;
  nationality: string;
  languagePreference: string;
  timeZone: string;
}

export interface UserLoginDto {
  email: string;
  password: string;
}

export interface UserLoginResponseDto {
  token: string;
  user: UserReadDto;
}

export interface UserReadDto {
  fullName: string;
  email: string;
  dateOfBirth?: Date;
  street: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  bio: string;
  alternatePhoneNumber: string;
  gender: number;
  nationality: string;
  languagePreference: string;
  timeZone: string;
}

export interface UserUpdateDto {
  userId: string; // Guid equivalente a string no TS
  fullName: string;
  email: string;
  street: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  bio: string;
  alternatePhoneNumber: string;
  gender: number;
  nationality: string;
  languagePreference: string;
  timeZone: string;
}
