import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgChartsModule } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

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
  revenueChartData: ChartData = {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    datasets: [{
      label: 'Revenue',
      data: [30000, 35000, 25000, 45000, 35000, 40000, 30000, 45000, 35000, 25000, 35000, 40000],
      borderColor: '#4154f1',
      backgroundColor: 'rgba(65, 84, 241, 0.1)',
      tension: 0.4,
      fill: true
    }]
  };

  salesChartData: ChartData = {
    labels: ['Direct', 'Affiliate', 'Sponsored', 'E-mail'],
    datasets: [{
      data: [40, 25, 20, 15],
      backgroundColor: [
        '#4154f1',
        '#6f42c1',
        '#2dce89',
        '#11cdef'
      ]
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

  pieChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: {
        position: 'right'
      }
    }
  };

  ngOnInit(): void {
    // Additional initialization if needed
  }
}