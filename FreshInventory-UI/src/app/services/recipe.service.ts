import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  RecipeCreateDto,
  RecipeReadDto,
  RecipeUpdateDto
} from '../models/recipe.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RecipeService {
  private apiUrl = `${environment.apiUrl}/api/Recipe`;

  constructor(private http: HttpClient) {}

  createRecipe(recipe: RecipeCreateDto): Observable<RecipeReadDto> {
    return this.http.post<RecipeReadDto>(`${this.apiUrl}/Create`, recipe);
  }

  getRecipeById(id: number): Observable<RecipeReadDto> {
    return this.http.get<RecipeReadDto>(`${this.apiUrl}/GetById/${id}`);
  }

  getAllRecipesPaged(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<{ data: RecipeReadDto[]; totalCount: number }> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<{ data: RecipeReadDto[]; totalCount: number }>(
      `${this.apiUrl}/GetAllPaged`,
      { params }
    );
  }

  updateRecipe(id: number, recipe: RecipeUpdateDto): Observable<RecipeReadDto> {
    return this.http.put<RecipeReadDto>(`${this.apiUrl}/Update/${id}`, recipe);
  }

  deleteRecipe(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Delete/${id}`);
  }
}
