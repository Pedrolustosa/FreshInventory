import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, from } from 'rxjs';
import { Recipe } from '../models/recipe.model';
import { environment } from '../../environments/environment';
import { IngredientService } from './ingredient.service';
import { Ingredient } from '../models/ingredient.model';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private apiUrl = `${environment.apiUrl}/api/recipes`;

  constructor(
    private http: HttpClient,
    private ingredientService: IngredientService
  ) {}

  getRecipes(): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(this.apiUrl).pipe(
      map(recipes => this.checkRecipesAvailability(recipes))
    );
  }

  getRecipeById(id: number): Observable<Recipe> {
    return this.http.get<Recipe>(`${this.apiUrl}/${id}`).pipe(
      map(recipe => this.checkRecipesAvailability([recipe])[0])
    );
  }

  createRecipe(recipe: Omit<Recipe, 'id' | 'createdDate' | 'updatedDate'>): Observable<Recipe> {
    return this.http.post<Recipe>(this.apiUrl, recipe);
  }

  updateRecipe(id: number, recipe: Partial<Recipe>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, recipe);
  }

  deleteRecipe(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  private checkRecipesAvailability(recipes: Recipe[]): Recipe[] {
    return recipes.map(recipe => ({
      ...recipe,
      isAvailable: recipe.ingredients.every(recipeIngredient => {
        const stockIngredient = this.ingredients.find((i: Ingredient) => i.id === recipeIngredient.ingredientId);
        return stockIngredient && stockIngredient.quantity >= recipeIngredient.quantity;
      })
    }));
  }

  private get ingredients(): Ingredient[] {
    let ingredients: Ingredient[] = [];
    this.ingredientService.getIngredients().subscribe(response => {
      ingredients = response.items;
    });
    return ingredients;
  }
}