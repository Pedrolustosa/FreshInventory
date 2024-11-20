import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Recipe } from '../models/recipe.model';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private apiUrl = `${environment.apiUrl}/api/Recipe`;

  constructor(private http: HttpClient) {}

  getRecipes(
    pageNumber: number = 1,
    pageSize: number = 10,
    name?: string,
    sortBy?: string,
    sortDirection?: string
  ): Observable<any> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (name) params = params.set('name', name);
    if (sortBy) params = params.set('sortBy', sortBy);
    if (sortDirection) params = params.set('sortDirection', sortDirection);

    return this.http.get<any>(`${this.apiUrl}/GetAllRecipes`, { params });
  }

  getRecipeById(id: number): Observable<Recipe> {
    return this.http.get<Recipe>(`${this.apiUrl}/GetRecipeById/${id}`);
  }

  createRecipe(recipe: Omit<Recipe, 'id' | 'createdDate' | 'updatedDate'>): Observable<Recipe> {
    return this.http.post<Recipe>(`${this.apiUrl}/CreateRecipe`, recipe);
  }

  updateRecipe(id: number, recipe: Partial<Recipe>): Observable<Recipe> {
    return this.http.put<Recipe>(`${this.apiUrl}/UpdateRecipe/${id}`, { ...recipe, id });
  }

  deleteRecipe(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteRecipe/${id}`);
  }

  reactivateRecipe(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/ReactivateRecipe/${id}`, {});
  }

  reserveIngredients(id: number): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/ReserveIngredientsForRecipe/${id}`, {});
  }
}