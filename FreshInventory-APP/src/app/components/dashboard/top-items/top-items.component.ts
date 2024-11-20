import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

interface TopItem {
  name: string;
  quantity: string;
  value: string;
}

@Component({
  selector: 'app-top-items',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './top-items.component.html',
  styleUrls: ['./top-items.component.css']
})
export class TopItemsComponent {
  @Input() title!: string;
  @Input() items!: TopItem[];
  @Input() type: 'success' | 'danger' = 'success';
  @Input() period?: string;
}