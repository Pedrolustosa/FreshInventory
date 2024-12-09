import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  IngredientCreateDto,
  IngredientReadDto,
  IngredientUpdateDto,
} from '../models/ingredient.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class IngredientService {
  private apiUrl = `${environment.apiUrl}/api/Ingredient`;

  constructor(private http: HttpClient) {}

  createIngredient(ingredient: IngredientCreateDto): Observable<IngredientReadDto> {
    return this.http.post<IngredientReadDto>(`${this.apiUrl}/Create`, ingredient);
  }

  getIngredientById(id: number): Observable<IngredientReadDto> {
    return this.http.get<IngredientReadDto>(`${this.apiUrl}/GetById/${id}`);
  }
  
  getAllIngredientsPaged(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<{ data: IngredientReadDto[]; totalCount: number }> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<{ data: IngredientReadDto[]; totalCount: number }>(
      `${this.apiUrl}/GetAllPaged`,
      { params }
    );
  }

  updateIngredient(id: number, ingredient: IngredientUpdateDto): Observable<IngredientReadDto> {
    return this.http.put<IngredientReadDto>(`${this.apiUrl}/Update/${id}`, ingredient);
  }

  deleteIngredient(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Delete/${id}`);
  }
}
