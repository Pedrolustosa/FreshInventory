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

  financialMetrics = {
    totalProfit: 0,
    netProfit: 0,
  };

  ingredientMetrics = {
    totalAvailableStock: 0,
    lowStockCount: 0,
  };

  customerMetrics = {
    totalCustomersServed: 0,
    repeatCustomers: 0,
  };

  wasteMetrics = {
    totalWasteCost: 0,
    spoiledProductsCount: 0,
  };

  dishMetrics = {
    leastProfitableDish: "None",
  };

  revenueChartData: ChartData = {
    labels: [
      "Jan",
      "Feb",
      "Mar",
      "Apr",
      "May",
      "Jun",
      "Jul",
      "Aug",
      "Sep",
      "Oct",
      "Nov",
      "Dec",
    ],
    datasets: [
      {
        label: "Revenue",
        data: [3000, 4000, 3500, 5000, 4500, 6000, 7000, 8000, 8500, 9000, 9500, 10000],
        borderColor: "rgba(65, 84, 241, 1)",
        backgroundColor: "rgba(65, 84, 241, 0.1)",
        tension: 0.4,
        fill: true,
      },
    ],
  };

  inventoryChartData: ChartData = {
    labels: ["Vegetables", "Fruits", "Dairy", "Meat", "Grains"],
    datasets: [
      {
        label: "Inventory Distribution",
        data: [40, 25, 15, 10, 10],
        backgroundColor: [
          "rgba(75, 192, 192, 0.6)",
          "rgba(153, 102, 255, 0.6)",
          "rgba(255, 159, 64, 0.6)",
          "rgba(54, 162, 235, 0.6)",
          "rgba(255, 205, 86, 0.6)",
        ],
        borderWidth: 1,
      },
    ],
  };

  chartOptions: ChartConfiguration["options"] = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: "top",
      },
    },
    scales: {
      y: {
        beginAtZero: true,
        grid: {
          color: "rgba(0, 0, 0, 0.05)",
        },
      },
      x: {
        grid: {
          display: false,
        },
      },
    },
  };

  constructor(
    private dashboardService: DashboardService,
    private toastService: ToastService,
    private spinnerService: SpinnerService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    this.spinnerService.show();

    forkJoin({
      financial: this.dashboardService.getFinancialMetrics(),
      ingredients: this.dashboardService.getIngredientMetrics(),
      customers: this.dashboardService.getCustomerMetrics(),
      waste: this.dashboardService.getWasteMetrics(),
      dishes: this.dashboardService.getDishMetrics(),
    })
      .pipe(
        catchError((error) => {
          this.toastService.error("Failed to load dashboard data");
          throw error;
        }),
        finalize(() => this.spinnerService.hide())
      )
      .subscribe((data) => {
        this.financialMetrics = data.financial;
        this.ingredientMetrics = data.ingredients;
        this.customerMetrics = data.customers;
        this.wasteMetrics = data.waste;
        this.dishMetrics = data.dishes;
      });
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat("en-US", {
      style: "currency",
      currency: "USD",
      minimumFractionDigits: 2,
    }).format(value);
  }
}
