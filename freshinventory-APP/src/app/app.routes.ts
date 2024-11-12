import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { IngredientListComponent } from './components/ingredient/list/ingredient-list.component';
import { IngredientCreateComponent } from './components/ingredient/create/ingredient-create.component';
import { IngredientUpdateComponent } from './components/ingredient/update/ingredient-update.component';
import { RecipeListComponent } from './components/recipe/list/recipe-list.component';
import { RecipeCreateComponent } from './components/recipe/create/recipe-create.component';
import { RecipeUpdateComponent } from './components/recipe/update/recipe-update.component';
import { ProfileComponent } from './components/profile/profile.component';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'auth/login', component: LoginComponent },
  { path: 'auth/register', component: RegisterComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'ingredients', component: IngredientListComponent },
  { path: 'ingredients/create', component: IngredientCreateComponent },
  { path: 'ingredients/update/:id', component: IngredientUpdateComponent },
  { path: 'recipes', component: RecipeListComponent },
  { path: 'recipes/create', component: RecipeCreateComponent },
  { path: 'recipes/update/:id', component: RecipeUpdateComponent },
  { path: 'profile', component: ProfileComponent },
  { path: '**', redirectTo: '/home' }
];