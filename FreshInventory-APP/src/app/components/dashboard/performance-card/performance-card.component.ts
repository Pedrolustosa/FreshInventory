import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

interface PerformanceMetric {
  title: string;
  value: string;
  target: string;
  progress: number;
  icon: string;
}

@Component({
  selector: 'app-performance-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './performance-card.component.html',
  styleUrls: ['./performance-card.component.css']
})
export class PerformanceCardComponent {
  @Input() title!: string;
  @Input() metrics!: PerformanceMetric[];

  getProgressColor(progress: number): string {
    if (progress >= 90) return 'success';
    if (progress >= 70) return 'warning';
    return 'danger';
  }
}