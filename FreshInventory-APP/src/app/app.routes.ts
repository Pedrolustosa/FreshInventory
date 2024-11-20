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
import { SupplierListComponent } from './components/supplier/list/supplier-list.component';
import { SupplierCreateComponent } from './components/supplier/create/supplier-create.component';
import { SupplierUpdateComponent } from './components/supplier/update/supplier-update.component';
import { ProfileComponent } from './components/profile/profile.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'auth/login', component: LoginComponent },
  { path: 'auth/register', component: RegisterComponent },
  { 
    path: 'dashboard', 
    component: DashboardComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'ingredients', 
    component: IngredientListComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'ingredients/create', 
    component: IngredientCreateComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'ingredients/update/:id', 
    component: IngredientUpdateComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'recipes', 
    component: RecipeListComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'recipes/create', 
    component: RecipeCreateComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'recipes/update/:id', 
    component: RecipeUpdateComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'suppliers', 
    component: SupplierListComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'suppliers/create', 
    component: SupplierCreateComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'suppliers/update/:id', 
    component: SupplierUpdateComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'profile', 
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },
  { path: '**', redirectTo: '/home' }
];