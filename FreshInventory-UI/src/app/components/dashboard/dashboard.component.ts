import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { NgChartsModule } from "ng2-charts";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { ChartConfiguration, ChartData } from "chart.js";
import { DashboardService } from "../../services/dashboard.service";
import { ToastService } from "../../services/toast.service";
import { SpinnerService } from "../../services/spinner.service";
import { catchError, finalize } from "rxjs/operators";
import { forkJoin } from "rxjs";

interface TopItem {
  name: string;
  category: string;
  quantity: number;
  sales: number;
  trend: 'up' | 'down' | 'neutral';
  percentage: number;
}

interface Activity {
  type: 'success' | 'warning' | 'danger' | 'info';
  icon: string;
  title: string;
  description: string;
  time: Date;
}

@Component({
  selector: "app-dashboard",
  standalone: true,
  imports: [CommonModule, NgChartsModule, BsDropdownModule],
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.css"],
})
export class DashboardComponent implements OnInit {
  userName: string = "Admin";
  today = new Date();

  // Metrics
  financialMetrics = {
    totalProfit: 0,
    netProfit: 0,
  };

  ingredientMetrics = {
    totalAvailableStock: 0,
    lowStockCount: 0,
  };

  // Chart Data
  salesChartData: ChartData = {
    labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
    datasets: [
      {
        label: 'Sales',
        data: [65, 59, 80, 81, 56, 55, 40],
        borderColor: 'rgb(45, 206, 137)',
        backgroundColor: 'rgba(45, 206, 137, 0.1)',
        fill: true,
        tension: 0.4,
      },
    ],
  };

  salesChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: false,
      },
      tooltip: {
        mode: 'index',
        intersect: false,
      },
    },
    interaction: {
      mode: 'nearest',
      axis: 'x',
      intersect: false
    },
    scales: {
      y: {
        beginAtZero: true,
        border: {
          display: false
        },
        grid: {
          color: 'rgba(0,0,0,0.1)',
        },
        ticks: {
          color: 'rgba(0,0,0,0.6)',
          maxTicksLimit: 5,
          callback: function(value: string | number) {
            const numValue = Number(value);
            return !isNaN(numValue) && numValue >= 1000 ? 
              (numValue / 1000).toFixed(1) + 'k' : 
              value;
          }
        }
      },
      x: {
        border: {
          display: false
        },
        grid: {
          display: false,
        },
        ticks: {
          color: 'rgba(0,0,0,0.6)',
          maxRotation: 0,
          autoSkipPadding: 15,
          maxTicksLimit: 7
        }
      },
    },
  };

  stockChartData: ChartData = {
    labels: ['Vegetables', 'Meat', 'Dairy', 'Grains', 'Others'],
    datasets: [
      {
        data: [30, 25, 20, 15, 10],
        backgroundColor: [
          'rgba(45, 206, 137, 0.8)',
          'rgba(65, 84, 241, 0.8)',
          'rgba(252, 185, 44, 0.8)',
          'rgba(250, 92, 124, 0.8)',
          'rgba(159, 122, 234, 0.8)',
        ],
      },
    ],
  };

  stockChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'bottom',
        labels: {
          usePointStyle: true,
          padding: 20,
          font: {
            size: 12
          }
        }
      },
      tooltip: {
        mode: 'index',
        intersect: false,
      }
    },
    layout: {
      padding: {
        top: 20,
        bottom: 20
      }
    }
  };

  // Sample Data
  topItems: TopItem[] = [
    {
      name: 'Fresh Tomatoes',
      category: 'Vegetables',
      quantity: 250,
      sales: 1200.50,
      trend: 'up',
      percentage: 15
    },
    {
      name: 'Chicken Breast',
      category: 'Meat',
      quantity: 180,
      sales: 2100.75,
      trend: 'down',
      percentage: 8
    },
    {
      name: 'Mozzarella',
      category: 'Dairy',
      quantity: 120,
      sales: 960.25,
      trend: 'up',
      percentage: 12
    },
    {
      name: 'Pasta',
      category: 'Grains',
      quantity: 300,
      sales: 750.00,
      trend: 'neutral',
      percentage: 0
    }
  ];

  recentActivities: Activity[] = [
    {
      type: 'success',
      icon: 'fa-box',
      title: 'New Stock Arrived',
      description: 'Fresh vegetables delivery received',
      time: new Date()
    },
    {
      type: 'warning',
      icon: 'fa-exclamation-triangle',
      title: 'Low Stock Alert',
      description: 'Chicken breast stock is running low',
      time: new Date(Date.now() - 3600000)
    },
    {
      type: 'info',
      icon: 'fa-chart-line',
      title: 'Sales Milestone',
      description: 'Monthly sales target achieved',
      time: new Date(Date.now() - 7200000)
    },
    {
      type: 'danger',
      icon: 'fa-times-circle',
      title: 'Items Expired',
      description: '3 dairy products marked as expired',
      time: new Date(Date.now() - 10800000)
    }
  ];

  constructor(
    private dashboardService: DashboardService,
    private toastService: ToastService,
    private spinnerService: SpinnerService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.spinnerService.show();

    forkJoin({
      financial: this.dashboardService.getFinancialMetrics(),
      inventory: this.dashboardService.getIngredientMetrics(),
    })
      .pipe(
        catchError((error) => {
          this.toastService.error(
            "Error loading dashboard data. Please try again."
          );
          throw error;
        }),
        finalize(() => this.spinnerService.hide())
      )
      .subscribe({
        next: (data) => {
          this.financialMetrics = data.financial;
          this.ingredientMetrics = data.inventory;
        },
      });
  }

  getTrendIcon(trend: string): string {
    switch (trend) {
      case 'up':
        return 'fa-arrow-up';
      case 'down':
        return 'fa-arrow-down';
      default:
        return 'fa-minus';
    }
  }
}
