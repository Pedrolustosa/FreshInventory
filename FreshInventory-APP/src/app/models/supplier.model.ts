export interface Supplier {
  id: number;
  name: string;
  email: string;
  phone: string;
  address: string;
  contactPerson: string;
  category: string;
  status: boolean;
  createdDate: Date;
  updatedDate: Date;
}

export interface CreateSupplier {
  name: string;
  email: string;
  phone: string;
  address: string;
  contactPerson: string;
  category: string;
  status: boolean;
}

export interface UpdateSupplier {
  id: number;
  name: string;
  email: string;
  phone: string;
  address: string;
  contactPerson: string;
  category: string;
  status: boolean;
}