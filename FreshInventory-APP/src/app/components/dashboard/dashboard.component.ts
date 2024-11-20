import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgChartsModule } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { DashboardService } from '../../services/dashboard.service';
import { SpinnerService } from '../../services/spinner.service';
import { ToastService } from '../../services/toast.service';
import { forkJoin } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import {
  WasteMetrics,
  FinancialMetrics,
  IngredientMetrics,
  DishMetrics,
  CustomerMetrics
} from '../../models/dashboard.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule, 
    NgChartsModule,
    BsDropdownModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  today = new Date();
  
  // Initialize metrics with default values
  wasteMetrics: WasteMetrics = {
    totalWastage: 0,
    spoiledProductsCount: 0,
    totalWasteCost: 0
  };

  financialMetrics: FinancialMetrics = {
    totalProfit: 0,
    totalExpenses: 0,
    netProfit: 0
  };

  ingredientMetrics: IngredientMetrics = {
    totalEntries: 0,
    totalAvailableStock: 0,
    lowStockCount: 0
  };

  dishMetrics: DishMetrics = {
    totalDishOutputs: 0,
    mostPopularDish: '',
    leastPopularDish: '',
    mostProfitableDish: '',
    leastProfitableDish: ''
  };

  customerMetrics: CustomerMetrics = {
    totalCustomersServed: 0,
    repeatCustomers: 0,
    averageSpendingPerCustomer: 0
  };

  revenueChartData: ChartData = {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    datasets: [{
      label: 'Revenue',
      data: Array(12).fill(0),
      borderColor: 'rgba(65, 84, 241, 1)',
      backgroundColor: 'rgba(65, 84, 241, 0.1)',
      tension: 0.4,
      fill: true
    }]
  };

  chartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: {
        display: false
      }
    },
    scales: {
      y: {
        beginAtZero: true,
        grid: {
          color: 'rgba(0, 0, 0, 0.05)'
        }
      },
      x: {
        grid: {
          display: false
        }
      }
    }
  };

  constructor(
    private dashboardService: DashboardService,
    private spinnerService: SpinnerService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    this.spinnerService.show();

    forkJoin({
      waste: this.dashboardService.getWasteMetrics(),
      financial: this.dashboardService.getFinancialMetrics(),
      ingredient: this.dashboardService.getIngredientMetrics(),
      dish: this.dashboardService.getDishMetrics(),
      customer: this.dashboardService.getCustomerMetrics()
    }).pipe(
      catchError(error => {
        this.toastService.error('Failed to load dashboard data');
        throw error;
      }),
      finalize(() => this.spinnerService.hide())
    ).subscribe({
      next: (data) => {
        this.wasteMetrics = data.waste;
        this.financialMetrics = data.financial;
        this.ingredientMetrics = data.ingredient;
        this.dishMetrics = data.dish;
        this.customerMetrics = data.customer;
        this.updateCharts();
      }
    });
  }

  private updateCharts(): void {
    // Update revenue chart with actual data
    // This is a placeholder - replace with actual data processing logic
    this.revenueChartData.datasets[0].data = Array(12).fill(0).map(() => 
      Math.floor(Math.random() * 50000) + 10000
    );
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 2
    }).format(value);
  }

  getLowStockMessage(): string {
    return this.ingredientMetrics.lowStockCount > 0
      ? `${this.ingredientMetrics.lowStockCount} items low`
      : 'All items stocked';
  }

  getSpoiledItemsMessage(): string {
    return this.wasteMetrics.spoiledProductsCount > 0
      ? `${this.wasteMetrics.spoiledProductsCount} items spoiled`
      : 'No waste';
  }

  getStockStatusClass(): string {
    return this.ingredientMetrics.lowStockCount > 0 ? 'warning' : 'success';
  }

  getWasteStatusClass(): string {
    return this.wasteMetrics.spoiledProductsCount > 0 ? 'danger' : 'success';
  }
}