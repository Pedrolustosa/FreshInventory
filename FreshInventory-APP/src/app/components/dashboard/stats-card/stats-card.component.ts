import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-stats-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './stats-card.component.html',
  styleUrls: ['./stats-card.component.css']
})
export class StatsCardComponent {
  @Input() title!: string;
  @Input() value!: string;
  @Input() change!: string;
  @Input() icon!: string;
  @Input() type!: string;
  @Input() trend!: string;
  @Input() description!: string;
}