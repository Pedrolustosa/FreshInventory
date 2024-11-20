import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgChartsModule } from 'ng2-charts';
import { ChartConfiguration, ChartType } from 'chart.js';

@Component({
  selector: 'app-chart-card',
  standalone: true,
  imports: [CommonModule, NgChartsModule],
  templateUrl: './chart-card.component.html',
  styleUrls: ['./chart-card.component.css']
})
export class ChartCardComponent {
  @Input() title!: string;
  @Input() subtitle?: string;
  @Input() chartData!: ChartConfiguration['data'];
  @Input() chartOptions!: ChartConfiguration['options'];
  @Input() chartType!: ChartType;
  @Input() period?: string;
}