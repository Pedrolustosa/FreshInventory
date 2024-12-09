import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
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
    return this.http.post<RecipeReadDto>(`${this.apiUrl}/Create`, recipe).pipe(
      catchError(this.handleError('Failed to create recipe'))
    );
  }

  getRecipeById(id: number): Observable<RecipeReadDto> {
    return this.http.get<RecipeReadDto>(`${this.apiUrl}/GetById/${id}`).pipe(
      catchError(this.handleError('Failed to fetch recipe by ID'))
    );
  }

  getAllRecipesPaged(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<{ data: RecipeReadDto[]; totalCount: number }> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http
      .get<{ data: RecipeReadDto[]; totalCount: number }>(`${this.apiUrl}/GetAllPaged`, {
        params,
      })
      .pipe(catchError(this.handleError('Failed to fetch recipes')));
  }

  updateRecipe(id: number, recipe: RecipeUpdateDto): Observable<RecipeReadDto> {
    return this.http.put<RecipeReadDto>(`${this.apiUrl}/Update/${id}`, recipe).pipe(
      catchError(this.handleError('Failed to update recipe'))
    );
  }

  deleteRecipe(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Delete/${id}`).pipe(
      catchError(this.handleError('Failed to delete recipe'))
    );
  }

  private handleError(message: string) {
    return (error: any) => {
      console.error(`${message}:`, error);
      return throwError(() => new Error(error?.error?.message || message));
    };
  }
}
