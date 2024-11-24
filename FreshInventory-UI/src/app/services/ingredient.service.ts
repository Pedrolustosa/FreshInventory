import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Ingredient, CreateIngredient, UpdateIngredient } from '../models/ingredient.model';

@Injectable({
  providedIn: 'root'
})
export class IngredientService {
  private apiUrl = `${environment.apiUrl}/api/Ingredients`;

  constructor(private http: HttpClient) {}

  getIngredients(
    pageNumber: number = 1,
    pageSize: number = 10,
    name?: string,
    category?: string,
    sortBy?: string,
    sortDirection?: string
  ): Observable<any> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (name) params = params.set('name', name);
    if (category) params = params.set('category', category);
    if (sortBy) params = params.set('sortBy', sortBy);
    if (sortDirection) params = params.set('sortDirection', sortDirection);

    return this.http.get<any>(`${this.apiUrl}/GetAllIngredients`, { params });
  }

  getIngredientById(id: number): Observable<Ingredient> {
    return this.http.get<Ingredient>(`${this.apiUrl}/GetIngredientById/${id}`);
  }

  createIngredient(ingredient: CreateIngredient): Observable<Ingredient> {
    return this.http.post<Ingredient>(`${this.apiUrl}/CreateIngredient`, ingredient);
  }

  updateIngredient(id: number, ingredient: UpdateIngredient): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/UpdateIngredient/${id}`, ingredient);
  }

  deleteIngredient(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteIngredient/${id}`);
  }
}