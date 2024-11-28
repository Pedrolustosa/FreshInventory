import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CreateRecipe, Recipe, RecipeCreateDto } from '../models/recipe.model';

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

  createRecipe(recipe: CreateRecipe): Observable<RecipeCreateDto> {
    const recipeDto: RecipeCreateDto = {
      Name: recipe.name,
      Category: recipe.category,
      PreparationTime: recipe.preparationTime.toString(),
      Servings: recipe.servings.toString(),
      Description: recipe.description,
      IsAvailable: recipe.isAvailable,
      Instructions: recipe.instructions,
      Ingredients: recipe.ingredients.map(ing => ({
        IngredientId: ing.ingredientId,
        Quantity: ing.quantity
      }))
    };
    return this.http.post<RecipeCreateDto>(`${this.apiUrl}/CreateRecipe`, recipeDto);
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