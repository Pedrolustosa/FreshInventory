export interface SupplierCreateDto {
  name: string;
  contact: string;
  email: string;
  phone: string;
  address: string;
  category: string;
  status: boolean;
}

export interface SupplierReadDto {
  id: number;
  name: string;
  contact: string;
  email: string;
  phone: string;
  address: string;
  category: string;
  status: boolean;
}

export interface SupplierUpdateDto {
  name: string;
  contact: string;
  email: string;
  phone: string;
  address: string;
  category: string;
  status: boolean;
}