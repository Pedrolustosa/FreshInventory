import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Supplier, CreateSupplier, UpdateSupplier } from '../models/supplier.model';

@Injectable({
  providedIn: 'root'
})
export class SupplierService {
  private apiUrl = `${environment.apiUrl}/api/Supplier`;

  constructor(private http: HttpClient) {}

  getSuppliers(
    pageNumber: number = 1,
    pageSize: number = 10,
    name?: string,
    sortBy?: string,
    sortDirection?: string
  ): Observable<Supplier> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (name) params = params.set('name', name);
    if (sortBy) params = params.set('sortBy', sortBy);
    if (sortDirection) params = params.set('sortDirection', sortDirection);

    return this.http.get<Supplier>(`${this.apiUrl}/GetAllSuppliers`, { params });
  }

  getSupplierById(id: number): Observable<Supplier> {
    return this.http.get<Supplier>(`${this.apiUrl}/GetSupplierById/${id}`);
  }

  createSupplier(supplier: CreateSupplier): Observable<Supplier> {
    return this.http.post<Supplier>(`${this.apiUrl}/CreateSupplier`, supplier);
  }

  updateSupplier(id: number, supplier: UpdateSupplier): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/UpdateSupplier/${id}`, supplier);
  }

  deleteSupplier(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteSupplier/${id}`);
  }
}