import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  WasteMetrics,
  FinancialMetrics,
  IngredientMetrics,
  DishMetrics,
  CustomerMetrics
} from '../models/dashboard.model';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = `${environment.apiUrl}/api/Dashboard`;

  constructor(private http: HttpClient) {}

  getWasteMetrics(): Observable<WasteMetrics> {
    return this.http.get<WasteMetrics>(`${this.apiUrl}/waste-metrics`);
  }

  getFinancialMetrics(): Observable<FinancialMetrics> {
    return this.http.get<FinancialMetrics>(`${this.apiUrl}/financial-metrics`);
  }

  getIngredientMetrics(): Observable<IngredientMetrics> {
    return this.http.get<IngredientMetrics>(`${this.apiUrl}/ingredient-metrics`);
  }

  getDishMetrics(): Observable<DishMetrics> {
    return this.http.get<DishMetrics>(`${this.apiUrl}/dish-metrics`);
  }

  getCustomerMetrics(): Observable<CustomerMetrics> {
    return this.http.get<CustomerMetrics>(`${this.apiUrl}/customer-metrics`);
  }
}