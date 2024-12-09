import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { NgChartsModule } from "ng2-charts";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { ChartConfiguration, ChartData } from "chart.js";
import { DashboardService } from "../../services/dashboard.service";
import { catchError, finalize } from "rxjs/operators";
import { forkJoin } from "rxjs";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";

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
      intersect: false,
    },
    scales: {
      y: {
        beginAtZero: true,
        border: {
          display: false,
        },
        grid: {
          color: 'rgba(0,0,0,0.1)',
        },
        ticks: {
          color: 'rgba(0,0,0,0.6)',
          maxTicksLimit: 5,
          callback: function (value: string | number) {
            const numValue = Number(value);
            return !isNaN(numValue) && numValue >= 1000
              ? (numValue / 1000).toFixed(1) + 'k'
              : value;
          },
        },
      },
      x: {
        border: {
          display: false,
        },
        grid: {
          display: false,
        },
        ticks: {
          color: 'rgba(0,0,0,0.6)',
          maxRotation: 0,
          autoSkipPadding: 15,
          maxTicksLimit: 7,
        },
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
            size: 12,
          },
        },
      },
      tooltip: {
        mode: 'index',
        intersect: false,
      },
    },
    layout: {
      padding: {
        top: 20,
        bottom: 20,
      },
    },
  };

  // Sample Data
  topItems: TopItem[] = [
    // Sample items
  ];

  recentActivities: Activity[] = [
    // Sample activities
  ];

  constructor(
    private dashboardService: DashboardService,
    private toastr: ToastrService, // Corrigido para usar ToastrService
    private spinner: NgxSpinnerService // Corrigido para usar NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.spinner.show();

    forkJoin({
      financial: this.dashboardService.getFinancialMetrics(),
      inventory: this.dashboardService.getIngredientMetrics(),
    })
      .pipe(
        catchError((error) => {
          this.toastr.error(
            'Error loading dashboard data. Please try again.'
          );
          throw error;
        }),
        finalize(() => this.spinner.hide())
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
