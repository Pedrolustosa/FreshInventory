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
  id: string;
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
  id: string;
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
