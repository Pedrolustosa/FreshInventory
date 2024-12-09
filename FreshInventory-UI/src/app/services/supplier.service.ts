import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  SupplierCreateDto,
  SupplierReadDto,
  SupplierUpdateDto,
} from '../models/supplier.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SupplierService {
  private apiUrl = `${environment.apiUrl}/api/Supplier`;

  constructor(private http: HttpClient) {}

  createSupplier(supplier: SupplierCreateDto): Observable<SupplierReadDto> {
    return this.http.post<SupplierReadDto>(`${this.apiUrl}/Create`, supplier);
  }

  getSupplierById(id: number): Observable<SupplierReadDto> {
    return this.http.get<SupplierReadDto>(`${this.apiUrl}/GetById/${id}`);
  }

  getAllSuppliersPaged(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<{ data: SupplierReadDto[]; totalCount: number }> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<{ data: SupplierReadDto[]; totalCount: number }>(
      `${this.apiUrl}/GetAllPaged`,
      { params }
    );
  }

  updateSupplier(
    id: number,
    supplier: SupplierUpdateDto
  ): Observable<SupplierReadDto> {
    return this.http.put<SupplierReadDto>(`${this.apiUrl}/Update/${id}`, supplier);
  }

  deleteSupplier(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Delete/${id}`);
  }
}
